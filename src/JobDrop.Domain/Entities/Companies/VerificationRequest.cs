using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Companies;

public sealed class VerificationRequest : BaseEntity
{
    public Guid CompanyId { get; set; }
    public VerificationLevel RequestedLevel { get; set; }
    public string EvidenceStorageKey { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public Guid? ReviewedByAdminId { get; set; }
    public string? ReviewNotes { get; set; }
    public DateTimeOffset RequestedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ReviewedAtUtc { get; set; }
}