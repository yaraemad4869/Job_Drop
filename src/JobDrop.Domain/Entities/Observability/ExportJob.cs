using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Observability;

public sealed class ExportJob : BaseEntity
{
    public Guid UserId { get; set; }
    public ExportKind Kind { get; set; }
    public ExportJobStatus Status { get; set; } = ExportJobStatus.Queued;
    public string? StorageKey { get; set; }
    public string? DownloadUrl { get; set; }
    public DateTimeOffset? DownloadUrlExpiresAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public string? FailureReason { get; set; }
}