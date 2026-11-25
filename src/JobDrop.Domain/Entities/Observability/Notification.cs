using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Observability;

public sealed class Notification : BaseEntity
{
    public Guid? UserId { get; set; }
    public Guid? CompanyId { get; set; }
    public NotificationChannel Channel { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string? DeepLink { get; set; }
    public bool IsRead { get; set; }
    public int RetryCount { get; set; }
    public DateTimeOffset? DeliveredAtUtc { get; set; }
    public DateTimeOffset? DeadLetteredAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}