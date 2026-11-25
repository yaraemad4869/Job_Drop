namespace JobDrop.Domain.Enums;

public enum SubscriptionStatus
{
    Active = 0,
    PastDue = 1,
    Cancelled = 2,
    Expired = 3,
    Trialing = 4
}