using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Observability;

public sealed class OutboxMessage : BaseEntity
{
    public string EventType { get; set; } = string.Empty;
    public string PayloadJson { get; set; } = "{}";
    public int RetryCount { get; set; }
    public DateTimeOffset NextAttemptAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ProcessedAtUtc { get; set; }
    public DateTimeOffset? DeadLetteredAtUtc { get; set; }
    public string? LastError { get; set; }
}