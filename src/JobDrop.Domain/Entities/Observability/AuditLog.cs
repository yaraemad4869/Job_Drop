using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Observability;

public sealed class AuditLog : BaseEntity
{
    public Guid? ActorUserId { get; set; }
    public ActorType ActorType { get; set; }
    public Guid? CompanyId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? TargetType { get; set; }
    public Guid? TargetId { get; set; }
    public string? OldValuesJson { get; set; }
    public string? NewValuesJson { get; set; }
    public string? CorrelationId { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}