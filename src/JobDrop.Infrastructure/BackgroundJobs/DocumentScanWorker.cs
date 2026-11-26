using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JobDrop.Infrastructure.BackgroundJobs;

/// <summary>
/// Magic-byte validation, malware scan, SHA-256, CV PII redaction.
/// </summary>
public sealed class DocumentScanWorker : BackgroundService
{
    private readonly ILogger<DocumentScanWorker> _logger;
    public DocumentScanWorker(ILogger<DocumentScanWorker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DocumentScanWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // TODO: Process StorageUploadIntents where Status == Uploaded
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex) { _logger.LogError(ex, "DocumentScanWorker error"); }
        }
    }
}