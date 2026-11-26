using JobDrop.Application.Contracts.Billing;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JobDrop.Infrastructure.BackgroundJobs;

/// <summary>
/// Polls ambiguous ProviderPending Paymob orders older than 15m.
/// Queries Paymob inquiry API to determine actual status.
/// Flags mismatches for Admin review.
/// </summary>
public sealed class PaymobReconcilerWorker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<PaymobReconcilerWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

    public PaymobReconcilerWorker(IServiceProvider services, ILogger<PaymobReconcilerWorker> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PaymobReconcilerWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ReconcileAsync(stoppingToken);
                await Task.Delay(_interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PaymobReconcilerWorker error");
                await Task.Delay(_interval, stoppingToken);
            }
        }
    }

    private async Task ReconcileAsync(CancellationToken ct)
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var paymob = scope.ServiceProvider.GetRequiredService<IPaymobClient>();

        // Find payments stuck in ProviderPending for > 15 minutes
        var cutoff = DateTime.UtcNow.AddMinutes(-15);
        var pending = await db.Payments
            .Where(p => p.Status == PaymentStatus.ProviderPending
                     && p.CreatedAtUtc < cutoff)
            .ToListAsync(ct);

        if (pending.Count == 0) return;

        _logger.LogInformation("Found {Count} pending payments to reconcile", pending.Count);

        foreach (var payment in pending)
        {
            try
            {
                // TODO: Call Paymob inquiry API
                // var status = await paymob.InquireAsync(payment.ProviderOrderId, ct);

                // For now, log for manual review
                _logger.LogWarning(
                    "Payment {PaymentId} stuck in ProviderPending since {CreatedAt}. Requires manual review.",
                    payment.Id,
                    payment.CreatedAtUtc);

                // Flag for admin review
                // TODO: Create AdminAlert or update payment status to ReconciliationRequired
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reconcile payment {PaymentId}", payment.Id);
            }
        }
    }
}