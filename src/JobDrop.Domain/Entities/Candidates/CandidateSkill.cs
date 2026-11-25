using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class CandidateSkill : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public Guid SkillId { get; set; }
    public ProficiencyLevel ProficiencyLevel { get; set; }
    public bool IsVerified { get; set; }
    public string? VerifiedBySource { get; set; }
}