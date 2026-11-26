using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JobDrop.Infrastructure.BackgroundJobs;

/// <summary>
/// Expires unaccepted ReverseDrops past 48h and refunds credit to company pool.
/// </summary>
public sealed class ReverseDropExpiryWorker : BackgroundService
{
    private readonly ILogger<ReverseDropExpiryWorker> _logger;
    public ReverseDropExpiryWorker(ILogger<ReverseDropExpiryWorker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ReverseDropExpiryWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // TODO: Find ReverseDrops where Status == Sent && ExpiresAtUtc <= now
                // TODO: Refund 1 credit to ReverseDropPool with idempotent key
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex) { _logger.LogError(ex, "ReverseDropExpiryWorker error"); }
        }
    }
}