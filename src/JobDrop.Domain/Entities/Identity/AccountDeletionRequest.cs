using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Identity;

public sealed class AccountDeletionRequest : BaseEntity
{
    public Guid UserId { get; set; }
    public DeletionRequestStatus Status { get; set; } = DeletionRequestStatus.Pending;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset RequestedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? GracePeriodEndsAtUtc { get; set; }
    public DateTimeOffset? StartedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public string? FailureReason { get; set; }
}