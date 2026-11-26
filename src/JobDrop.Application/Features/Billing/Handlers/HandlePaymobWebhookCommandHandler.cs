using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using JobDrop.Application.Contracts.Billing;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// Namespace aliases to avoid conflicts
using PaymentEntity = global::JobDrop.Domain.Entities.Billing.Payment;
using WebhookEventEntity = global::JobDrop.Domain.Entities.Billing.PaymentWebhookEvent;
using SubscriptionEntity = global::JobDrop.Domain.Entities.Billing.Subscription;

namespace JobDrop.Application.Features.Billing.Handlers;

public sealed class HandlePaymobWebhookCommandHandler : IRequestHandler<Commands.HandlePaymobWebhookCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly IPaymobClient _paymob;
    private readonly PaymobOptions _paymobOptions;
    private readonly ILogger<HandlePaymobWebhookCommandHandler> _logger;

    public HandlePaymobWebhookCommandHandler(
        IApplicationDbContext db,
        IPaymobClient paymob,
        IOptions<PaymobOptions> paymobOptions,
        ILogger<HandlePaymobWebhookCommandHandler> logger)
    {
        _db = db;
        _paymob = paymob;
        _paymobOptions = paymobOptions.Value;
        _logger = logger;
    }

    public async Task<bool> Handle(Commands.HandlePaymobWebhookCommand request, CancellationToken cancellationToken)
    {
        // 1. Enforce 64KB body limit
        if (request.RawPayload.Length > 64 * 1024)
        {
            _logger.LogWarning("Webhook payload exceeds 64KB limit");
            return false;
        }

        // 2. Cryptographic HMAC-SHA512 Verification (Timing-Safe)
        var secretBytes = Encoding.UTF8.GetBytes(_paymobOptions.HmacSecret);
        var payloadBytes = Encoding.UTF8.GetBytes(request.RawPayload);
        var expectedHashBytes = HMACSHA512.HashData(secretBytes, payloadBytes);
        
        byte[] providedHashBytes;
        try 
        {
            providedHashBytes = Convert.FromHexString(request.Signature);
        }
        catch (FormatException)
        {
            _logger.LogWarning("Invalid signature format provided by Paymob");
            return false;
        }

        // FIXED-TIME COMPARISON to prevent Timing Attacks
        if (!CryptographicOperations.FixedTimeEquals(expectedHashBytes, providedHashBytes))
        {
            _logger.LogWarning("HMAC validation failed (timing-safe comparison).");
            return false;
        }

        // 3. Parse Payload and STRICTLY bind to the signed 'order.id'
        using var jsonDoc = JsonDocument.Parse(request.RawPayload);
        var root = jsonDoc.RootElement;

        if (!root.TryGetProperty("order", out var orderElement) || 
            !orderElement.TryGetProperty("id", out var orderIdElement))
        {
            _logger.LogWarning("Missing 'order.id' in webhook payload");
            return false;
        }

        var providerOrderId = orderIdElement.GetString();
        var isSuccess = root.GetProperty("success").GetBoolean();
        var transactionId = root.GetProperty("transaction").GetProperty("id").GetString();

        // 4. Find payment by the SIGNED providerOrderId (NEVER trust merchant_order_id)
        var payment = await _db.Payments
            .FirstOrDefaultAsync(p => p.ProviderOrderId == providerOrderId, cancellationToken);

        if (payment == null)
        {
            _logger.LogWarning("Payment not found for signed order.id: {OrderId}", providerOrderId);
            return false;
        }

        // 5. Idempotent state transition
        var stateBucket = isSuccess ? "Success" : "Failed";
        var dedupKey = $"paymob:{transactionId}:{stateBucket}";

        var existingEvent = await _db.PaymentWebhookEvents
            .AnyAsync(e => e.EventStateKey == dedupKey, cancellationToken);

        if (existingEvent)
        {
            _logger.LogInformation("Webhook already processed (idempotent): {DedupKey}", dedupKey);
            return true;
        }

        // 6. Update Payment Status
        if (isSuccess)
        {
            payment.MarkPaid(transactionId);
        }
        else
        {
            payment.MarkFailed();
        }

        // 7. Record Webhook Event using default constructor + property setters
        var webhookEvent = new WebhookEventEntity();
        webhookEvent.EventStateKey = dedupKey;
        webhookEvent.ProviderTransactionId = transactionId;
        webhookEvent.PayloadHash = ComputeSha256(request.RawPayload);
        
        _db.PaymentWebhookEvents.Add(webhookEvent);

        // 8. Activate Subscription Atomically (if success)
        if (isSuccess && payment.SubscriptionId.HasValue)
        {
            var subscription = await _db.Subscriptions
                .FirstOrDefaultAsync(s => s.Id == payment.SubscriptionId, cancellationToken);
            subscription?.Activate();
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Webhook processed successfully for order: {OrderId}", providerOrderId);
        return true;
    }

    private static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}

public sealed class PaymobOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string HmacSecret { get; set; } = string.Empty;
    public string IntegrationId { get; set; } = string.Empty;
}