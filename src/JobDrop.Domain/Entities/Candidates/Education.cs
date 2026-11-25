using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class Education : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public string DegreeBadge { get; set; } = string.Empty;
    public Guid? InstitutionId { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public string? Faculty { get; set; }
    public string FieldOfStudy { get; set; } = string.Empty;
    public int GraduationYear { get; set; }
    public Guid? EvidenceDocumentId { get; set; }
    public bool IsVerified { get; set; }
}