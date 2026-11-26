using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Billing;

public sealed class Subscription : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid PlanId { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTimeOffset CurrentPeriodStartUtc { get; set; }
    public DateTimeOffset CurrentPeriodEndUtc { get; set; }
    public bool AutoRenew { get; set; } = true;
    public DateTimeOffset? CancelledAtUtc { get; set; }
    public string? ProviderSubscriptionId { get; set; }

    public void Activate()
    {
        Status = JobDrop.Domain.Enums.SubscriptionStatus.Active;
    }
}
