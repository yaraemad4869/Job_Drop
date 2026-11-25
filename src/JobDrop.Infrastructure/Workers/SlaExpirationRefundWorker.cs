using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Infrastructure.Workers;

public sealed class SlaExpirationRefundWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SlaExpirationRefundWorker> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

    public SlaExpirationRefundWorker(IServiceProvider serviceProvider, ILogger<SlaExpirationRefundWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SLA Expiration Refund Worker is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                var now = DateTime.UtcNow;

                // Find all pending SLAs that have expired
                var expiredSlas = await context.ApplicationSlas
                    .Where(s => s.Status == SlaStatus.Pending && s.DueAtUtc < now)
                    .ToListAsync(stoppingToken);

                foreach (var sla in expiredSlas)
                {
                    _logger.LogInformation("Processing expired SLA: {ApplicationId}", sla.ApplicationId);

                    // 1. Mark as ExpiredRefunded
                    sla.MarkExpiredRefunded();

                    // 2. Fetch Application to get CandidateProfileId
                    var application = await context.Applications
                        .FirstOrDefaultAsync(a => a.Id == sla.ApplicationId, stoppingToken);
                    
                    if (application != null)
                    {
                        // 3. Fetch Wallet and Refund
                        var wallet = await context.DropWalletBalances
                            .FirstOrDefaultAsync(w => w.CandidateProfileId == application.CandidateProfileId, stoppingToken);

                        if (wallet != null)
                        {
                            // Refund the credit (assuming Daily bucket for simplicity)
                            wallet.RefundCredit(WalletBucket.Daily); 
                            context.DropWalletBalances.Update(wallet);

                            // 4. Create Ledger Entry for Refund
                            var refundLedger = new DropLedgerEntry(
                                application.CandidateProfileId,
                                WalletBucket.Daily,
                                1, // Positive delta for refund
                                "SLA_EXPIRED_REFUND",
                                sla.ApplicationId.ToString(),
                                $"REFUND_{sla.Id}"
                            );
                            context.DropLedgerEntries.Add(refundLedger);
                        }
                    }

                    // 5. Decrement Job Intake Count since the drop is voided
                    if (application != null)
                    {
                        var job = await context.Jobs.FindAsync(new object[] { application.JobId }, stoppingToken);
                        job?.DecrementIntake();
                    }
                }

                if (expiredSlas.Count > 0)
                {
                    await context.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Successfully processed {Count} expired SLAs.", expiredSlas.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing SLA expirations.");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("SLA Expiration Refund Worker is stopping.");
    }
}
