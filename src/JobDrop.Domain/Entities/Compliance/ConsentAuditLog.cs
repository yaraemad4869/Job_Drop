using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Compliance;

public sealed class ConsentAuditLog : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? JobId { get; set; }
    public ConsentType ConsentType { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string ConsentVersion { get; set; } = string.Empty;
    public DateTimeOffset ConsentedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}