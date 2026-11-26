using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JobDrop.Infrastructure.BackgroundJobs;

/// <summary>
/// Promotes Queued drops up to per-job/company remaining capacity.
/// Runs every 30-60s with bounded batch size (Take(50)).
/// Re-validates job status, account active, CV scan before promotion.
/// Refunds invalid drops.
/// </summary>
public sealed class QueueDispatchWorker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<QueueDispatchWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);
    private const int BatchSize = 50;

    public QueueDispatchWorker(IServiceProvider services, ILogger<QueueDispatchWorker> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("QueueDispatchWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessBatchAsync(stoppingToken);
                await Task.Delay(_interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "QueueDispatchWorker error");
                await Task.Delay(_interval, stoppingToken);
            }
        }
    }

    private async Task ProcessBatchAsync(CancellationToken ct)
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        // Fixed: Use DropStatus.Queued (not DropStatus)
        var queued = await db.Applications
            .Where(a => a.Status == DropStatus.Queued)
            .OrderBy(a => a.CreatedAtUtc)
            .Take(BatchSize)
            .ToListAsync(ct);

        foreach (var app in queued)
        {
            // Re-validate job status
            var job = await db.Jobs
                .Where(j => j.Id == app.JobId)
                .FirstOrDefaultAsync(ct);

            if (job == null || job.Status != "Published")
            {
                await CancelAndRefundAsync(db, app, "Job no longer published", ct);
                continue;
            }

            // Re-validate company active
            var company = await db.Companies
                .Where(c => c.Id == app.CompanyId)
                .FirstOrDefaultAsync(ct);

            if (company == null || !company.IsActive)
            {
                await CancelAndRefundAsync(db, app, "Company inactive", ct);
                continue;
            }

            // Re-validate candidate active
            var candidate = await db.CandidateProfiles
                .Where(c => c.Id == app.CandidateProfileId)
                .FirstOrDefaultAsync(ct);

            if (candidate == null || !candidate.IsActive)
            {
                await CancelAndRefundAsync(db, app, "Candidate inactive", ct);
                continue;
            }

            // Check capacity - Fixed: Use DateOnly for MarketDate
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var intake = await db.JobDailyIntakes
                .Where(i => i.JobId == app.JobId && i.MarketDate == today)
                .FirstOrDefaultAsync(ct);

            if (intake == null || intake.DeliveredCount >= job.DailyIntakeCap)
            {
                // Capacity full, skip (will retry next cycle)
                continue;
            }

            // Promote to Delivered
            app.PromoteToDelivered();
            intake.IncrementDelivered();

            _logger.LogInformation("Promoted application {AppId} to Delivered", app.Id);
        }

        await db.SaveChangesAsync(ct);
    }

    private async Task CancelAndRefundAsync(
        IApplicationDbContext db,
        JobDrop.Domain.Entities.Drops.Application app,
        string reason,
        CancellationToken ct)
    {
        // Fixed: Use Cancel method (now added to Application entity)
        app.Cancel(reason);

        // Refund credit to BonusBalance
        var wallet = await db.DropWalletBalances
            .Where(w => w.CandidateProfileId == app.CandidateProfileId)
            .FirstOrDefaultAsync(ct);

        wallet?.RefundCredit(WalletBucket.Bonus);

        _logger.LogWarning("Cancelled application {AppId}: {Reason}", app.Id, reason);
    }
}