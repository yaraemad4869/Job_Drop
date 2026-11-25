using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class SavedRegion : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public Guid OperatingAreaId { get; set; }
    public string PreferredModesJson { get; set; } = "[]";
    public string ModeMaxSecondsJson { get; set; } = "{}";
}