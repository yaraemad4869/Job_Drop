using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Drops;

public sealed class DropLedgerEntry : BaseEntity
{
    public Guid CandidateProfileId { get; private set; }
    public WalletBucket Bucket { get; private set; }
    public int Delta { get; private set; }
    public string ReasonCode { get; private set; } = string.Empty;
    public string? ReferenceId { get; private set; }
    public string IdempotencyKey { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }

    private DropLedgerEntry() { }

    public DropLedgerEntry(Guid candidateProfileId, WalletBucket bucket, int delta, string reasonCode, string? referenceId, string idempotencyKey)
    {
        CandidateProfileId = candidateProfileId;
        Bucket = bucket;
        Delta = delta;
        ReasonCode = reasonCode;
        ReferenceId = referenceId;
        IdempotencyKey = idempotencyKey;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
