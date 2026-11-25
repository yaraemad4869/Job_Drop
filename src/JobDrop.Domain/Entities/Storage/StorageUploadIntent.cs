using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Storage;

public sealed class StorageUploadIntent : BaseEntity
{
    public Guid UserId { get; set; }
    public DocumentKind TargetKind { get; set; }
    public string StorageKey { get; set; } = string.Empty;
    public string ExpectedMimeType { get; set; } = string.Empty;
    public long MaxSizeBytes { get; set; }
    public UploadIntentStatus Status { get; set; } = UploadIntentStatus.Issued;
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}