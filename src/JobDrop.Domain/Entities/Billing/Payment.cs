using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Billing;

public sealed class Payment : ConcurrentAuditableEntity
{
    public Guid? SubscriptionId { get; set; }
    public Guid UserId { get; set; }
    public Guid? CompanyId { get; set; }
    public string MerchantReference { get; set; } = string.Empty;
    public string? ProviderOrderId { get; set; }
    public string? ProviderIntentionId { get; set; }
    public string? ProviderTransactionId { get; set; }
    public long AmountCents { get; set; }
    public string Currency { get; set; } = "USD";
    public PaymentStatus Status { get; set; } = PaymentStatus.Created;
    public string? FailureReason { get; set; }
    public DateTimeOffset? PaidAtUtc { get; set; }

    public void MarkPaid(string providerTransactionId)
    {
        Status = JobDrop.Domain.Enums.PaymentStatus.Paid;
        ProviderTransactionId = providerTransactionId;
    }

    public void MarkFailed()
    {
        Status = JobDrop.Domain.Enums.PaymentStatus.Failed;
    }

    public void SetProviderIds(string providerOrderId, string providerIntentionId)
    {
        ProviderOrderId = providerOrderId;
        ProviderIntentionId = providerIntentionId;
        Status = JobDrop.Domain.Enums.PaymentStatus.ProviderPending;
    }
}
