using System.Security.Cryptography;
using System.Text;
using JobDrop.Application.Contracts.Billing;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Billing;
using JobDrop.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobDrop.Application.Features.Billing.Handlers;

public sealed class HandlePaymobWebhookCommandHandler
    : IRequestHandler<Commands.HandlePaymobWebhookCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly IPaymobClient _paymob;
    private readonly ILogger<HandlePaymobWebhookCommandHandler> _logger;

    public HandlePaymobWebhookCommandHandler(
        IApplicationDbContext db,
        IPaymobClient paymob,
        ILogger<HandlePaymobWebhookCommandHandler> logger)
    {
        _db = db;
        _paymob = paymob;
        _logger = logger;
    }

    public async Task<bool> Handle(
        Commands.HandlePaymobWebhookCommand request,
        CancellationToken cancellationToken)
    {
        if (request.RawPayload.Length > 64 * 1024)
        {
            _logger.LogWarning("Webhook payload exceeds 64KB limit");
            return false;
        }

        var payload = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(request.RawPayload);
        if (payload == null) return false;

        if (!payload.TryGetValue("order_id", out var orderIdObj) ||
            !payload.TryGetValue("transaction_id", out var txIdObj) ||
            !payload.TryGetValue("success", out var successObj))
        {
            return false;
        }

        var providerOrderId = orderIdObj.ToString()!;
        var transactionId = txIdObj.ToString()!;
        var isSuccess = Convert.ToBoolean(successObj);

        // HMAC verification
        var secret = "PAYMOB_HMAC_SECRET";
        var isValid = await _paymob.VerifyHmacAsync(request.RawPayload, request.Signature, secret, cancellationToken);
        if (!isValid)
        {
            _logger.LogWarning("HMAC verification failed");
            return false;
        }

        var payment = await _db.Payments
            .FirstOrDefaultAsync(p => p.ProviderOrderId == providerOrderId, cancellationToken);

        if (payment == null)
        {
            _logger.LogWarning("Payment not found for order {OrderId}", providerOrderId);
            return false;
        }

        var stateBucket = isSuccess ? "Success" : "Failed";
        var dedupKey = $"paymob:{transactionId}:{stateBucket}";

        var existingEvent = await _db.PaymentWebhookEvents
            .AnyAsync(e => e.EventStateKey == dedupKey, cancellationToken);

        if (existingEvent)
        {
            _logger.LogInformation("Webhook already processed: {DedupKey}", dedupKey);
            return true;
        }

        if (isSuccess)
        {
            payment.MarkPaid(transactionId);
        }
        else
        {
            payment.MarkFailed();
        }

        // Create webhook event using default constructor
        var webhookEvent = new PaymentWebhookEvent();
        webhookEvent.Provider = "Paymob";
        webhookEvent.ProviderTransactionId = transactionId;
        webhookEvent.EventStateKey = dedupKey;
        webhookEvent.PayloadHash = ComputeSha256(request.RawPayload);

        _db.PaymentWebhookEvents.Add(webhookEvent);

        if (isSuccess && payment.SubscriptionId.HasValue)
        {
            var subscription = await _db.Subscriptions
                .FirstOrDefaultAsync(s => s.Id == payment.SubscriptionId, cancellationToken);
            subscription?.Activate();
        }

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}