namespace JobDrop.Domain.Enums;

public enum PaymentStatus
{
    Created = 0,
    ProviderPending = 1,
    Paid = 2,
    Refunded = 3,
    Voided = 4,
    Failed = 5
}