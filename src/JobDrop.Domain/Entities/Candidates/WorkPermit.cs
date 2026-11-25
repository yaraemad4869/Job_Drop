using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class WorkPermit : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public string MarketCode { get; set; } = string.Empty;
    public string PermitType { get; set; } = string.Empty;
    public string? Details { get; set; }
    public bool NeedsSponsorship { get; set; }
}