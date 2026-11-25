using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Drops;

public sealed class DropWalletBalance : ConcurrentAuditableEntity
{
    public Guid CandidateProfileId { get; set; }
    public int DailyRemaining { get; set; }
    public int DailyLimit { get; set; } = 5;
    public DateTimeOffset DailyResetAtUtc { get; set; }
    public int BonusBalance { get; set; }
    public int SpecialBalance { get; set; }

    public Candidates.CandidateProfile? CandidateProfile { get; set; }
}