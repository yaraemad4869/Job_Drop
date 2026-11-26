using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobDrop.Infrastructure.BackgroundJobs;

/// <summary>
/// Background worker to evaluate 48h SLA deadlines and auto-refund Drop credits.
/// Runs on a lease-based schedule to ensure idempotency.
/// </summary>
public sealed class SlaExpirationRefundWorker : BackgroundService
{
    private readonly ILogger<SlaExpirationRefundWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

    public SlaExpirationRefundWorker(ILogger<SlaExpirationRefundWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SlaExpirationRefundWorker is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // TODO: 1. Query DB for ApplicationSla where Status == Pending && DueAtUtc <= UtcNow
                // TODO: 2. Acquire distributed lease / DB lock to prevent concurrent refunds
                // TODO: 3. Dispatch RefundExpiredSlaCommand via MediatR or direct DbContext update
                // TODO: 4. Publish Outbox Notification for candidate
                
                await Task.Delay(_interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in SlaExpirationRefundWorker");
                await Task.Delay(_interval, stoppingToken);
            }
        }

        _logger.LogInformation("SlaExpirationRefundWorker is stopping.");
    }
}
