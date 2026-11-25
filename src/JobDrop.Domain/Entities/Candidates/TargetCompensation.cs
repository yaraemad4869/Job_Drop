using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class TargetCompensation : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public Guid? PositionId { get; set; }
    public string? PositionName { get; set; }
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Period { get; set; } = "Monthly";
}