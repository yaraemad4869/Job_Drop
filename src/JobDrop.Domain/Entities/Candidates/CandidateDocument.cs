using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class CandidateDocument : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public DocumentKind Kind { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public string? RedactedStorageKey { get; set; }
    public string Sha256Hash { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public ScanStatus ScanStatus { get; set; } = ScanStatus.PendingScan;
    public int Version { get; set; } = 1;
    public DateTimeOffset UploadedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}