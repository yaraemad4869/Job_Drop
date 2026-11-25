using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Billing;

public sealed class PaymentWebhookEvent : BaseEntity
{
    public string Provider { get; set; } = string.Empty;
    public string ProviderTransactionId { get; set; } = string.Empty;
    public string EventStateKey { get; set; } = string.Empty;
    public string PayloadHash { get; set; } = string.Empty;
    public string ProcessingStatus { get; set; } = "Pending";
    public string? RawPayload { get; set; }
    public DateTimeOffset ReceivedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ProcessedAtUtc { get; set; }
}