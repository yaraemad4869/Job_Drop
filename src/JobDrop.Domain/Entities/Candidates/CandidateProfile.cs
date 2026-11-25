using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Entities.Identity;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class CandidateProfile : ConcurrentAuditableEntity
{
    public Guid UserId { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string PreferredWorkModesJson { get; set; } = "[]";
    public string SharingProtocolJson { get; set; } = "{}";
    public string DropThemeCode { get; set; } = "classic";
    public bool IsOptedInTalentSearch { get; set; } = true;
    public decimal ProfileCompletenessPct { get; set; }

    public AppUser? User { get; set; }
    public ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
    public ICollection<Education> Educations { get; set; } = new List<Education>();
    public ICollection<CandidateSkill> Skills { get; set; } = new List<CandidateSkill>();
    public ICollection<CandidateLanguage> Languages { get; set; } = new List<CandidateLanguage>();
    public ICollection<CandidateDocument> Documents { get; set; } = new List<CandidateDocument>();
    public ICollection<StartingPoint> StartingPoints { get; set; } = new List<StartingPoint>();
    public ICollection<SavedRegion> SavedRegions { get; set; } = new List<SavedRegion>();
    public ICollection<TargetCompensation> TargetCompensations { get; set; } = new List<TargetCompensation>();
    public ICollection<WorkPermit> WorkPermits { get; set; } = new List<WorkPermit>();
}