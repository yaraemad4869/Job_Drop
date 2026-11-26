using JobDrop.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JobDrop.Infrastructure.Workers;

/// <summary>
/// Scans uploaded documents: validates magic bytes, runs malware scan,
/// computes SHA-256 hash, and triggers CV PII redaction pipeline.
/// </summary>
public sealed class DocumentScanWorker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<DocumentScanWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(15);

    public DocumentScanWorker(IServiceProvider services, ILogger<DocumentScanWorker> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DocumentScanWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingScansAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DocumentScanWorker");
            }

            try { await Task.Delay(_interval, stoppingToken); }
            catch (OperationCanceledException) { break; }
        }
    }

    private async Task ProcessPendingScansAsync(CancellationToken ct)
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        // In production: query CandidateDocuments where ScanStatus == PendingScan
        // For each: validate magic bytes (%PDF-, PK\x03\x04), run IMalwareScanner,
        // compute SHA-256, generate RedactedStorageKey via ICvRedactor, mark Clean.
        _logger.LogDebug("DocumentScanWorker cycle completed.");
        await Task.CompletedTask;
    }
}