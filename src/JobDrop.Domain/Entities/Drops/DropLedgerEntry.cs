using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Drops;

public sealed class DropLedgerEntry : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public WalletBucket Bucket { get; set; }
    public int Delta { get; set; }
    public string ReasonCode { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public int RunningBalanceAfter { get; set; }
}