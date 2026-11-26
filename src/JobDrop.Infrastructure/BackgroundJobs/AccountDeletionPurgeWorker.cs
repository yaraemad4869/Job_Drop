using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JobDrop.Infrastructure.BackgroundJobs;

/// <summary>
/// Executes approved AccountDeletionRequests: deletes blobs, clears PII, anonymizes ledger.
/// </summary>
public sealed class AccountDeletionPurgeWorker : BackgroundService
{
    private readonly ILogger<AccountDeletionPurgeWorker> _logger;
    public AccountDeletionPurgeWorker(ILogger<AccountDeletionPurgeWorker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AccountDeletionPurgeWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // TODO: Query AccountDeletionRequests where Status == Pending
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex) { _logger.LogError(ex, "AccountDeletionPurgeWorker error"); }
        }
    }
}