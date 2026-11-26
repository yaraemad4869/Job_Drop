using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SlaEntity = global::JobDrop.Domain.Entities.Drops.ApplicationSla;

namespace JobDrop.Infrastructure.Workers;

/// <summary>
/// Finds expired, unanswered SLAs, marks the drop ExpiredRefunded, refunds 1 credit
/// to BonusBalance and writes an idempotent DropLedgerEntry (refund:sla:{applicationId}).
/// Bounded batch + single SaveChanges = atomic unit of work per cycle.
/// </summary>
public sealed class SlaExpirationRefundWorker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<SlaExpirationRefundWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);
    private const int BatchSize = 50;

    public SlaExpirationRefundWorker(IServiceProvider services, ILogger<SlaExpirationRefundWorker> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SlaExpirationRefundWorker is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessExpiredSlasAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing SLA expirations.");
            }

            try { await Task.Delay(_interval, stoppingToken); }
            catch (OperationCanceledException) { break; }
        }
    }

    private async Task ProcessExpiredSlasAsync(CancellationToken ct)
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var walletPolicy = scope.ServiceProvider.GetRequiredService<DropWalletPolicy>();

        var nowUtc = DateTime.UtcNow;

        // DueAtUtc lives on ApplicationSla, NOT on Application.
        var expiredSlas = await db.ApplicationSlas
            .Where(s => s.Status == SlaStatus.Pending
                     && s.RespondedAtUtc == null
                     && s.DueAtUtc <= nowUtc)
            .OrderBy(s => s.DueAtUtc)
            .Take(BatchSize)
            .ToListAsync(ct);

        if (expiredSlas.Count == 0) return;

        _logger.LogInformation("Found {Count} expired SLA(s) to refund.", expiredSlas.Count);

        foreach (var sla in expiredSlas)
        {
            ct.ThrowIfCancellationRequested();

            var application = await db.Applications
                .FirstOrDefaultAsync(a => a.Id == sla.ApplicationId, ct);

            if (application is null)
            {
                _logger.LogWarning("SLA {SlaId} has no Application {ApplicationId}.", sla.Id, sla.ApplicationId);
                continue;
            }

            // Terminal-state guard (acts as a logical lease against re-processing).
            if (application.Status != DropStatus.Delivered &&
                application.Status != DropStatus.Opened &&
                application.Status != DropStatus.Queued)
            {
                sla.MarkExpired();
                continue;
            }

            var ledgerKey = $"refund:sla:{application.Id}";

            var alreadyRefunded = await db.DropLedgerEntries
                .AnyAsync(e => e.IdempotencyKey == ledgerKey, ct);

            if (alreadyRefunded)
            {
                sla.MarkExpired();
                continue;
            }

            var wallet = await db.DropWalletBalances
                .FirstOrDefaultAsync(w => w.CandidateProfileId == application.CandidateProfileId, ct);

            if (wallet is null)
            {
                _logger.LogWarning("Wallet missing for candidate {CandidateId}.", application.CandidateProfileId);
                continue;
            }

            application.Status = DropStatus.ExpiredRefunded;
            sla.MarkExpired();
            walletPolicy.RefundCredit(wallet, WalletBucket.Bonus);

            db.DropLedgerEntries.Add(new DropLedgerEntry(
                candidateProfileId: application.CandidateProfileId,
                bucket: WalletBucket.Bonus,
                delta: 1,
                reasonCode: "SlaExpiredRefund",
                referenceId: application.Id.ToString(),
                idempotencyKey: ledgerKey));

            _logger.LogInformation("Refunded expired SLA for Application {ApplicationId}.", application.Id);
        }

        await db.SaveChangesAsync(ct);
    }
}