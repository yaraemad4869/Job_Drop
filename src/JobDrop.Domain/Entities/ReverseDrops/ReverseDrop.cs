using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.ReverseDrops;

public sealed class ReverseDrop : ConcurrentAuditableEntity
{
    public Guid CompanyId { get; set; }
    public Guid JobId { get; set; }
    public Guid SenderUserId { get; set; }
    public Guid CandidateProfileId { get; set; }
    public string? OfferDocumentKey { get; set; }
    public string? DispatchNote { get; set; }
    public bool IsSalaryNegotiable { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string SalaryCurrency { get; set; } = "USD";
    public ReverseDropStatus Status { get; set; } = ReverseDropStatus.Sent;
    public DateTimeOffset SentAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset? RespondedAtUtc { get; set; }
    public string? IdempotencyKey { get; set; }

    public Companies.Company? Company { get; set; }
    public Jobs.Job? Job { get; set; }

    public void Accept()
    {
        Status = JobDrop.Domain.Enums.ReverseDropStatus.Accepted;
    }

        public void Decline()
    {
        if (Status != JobDrop.Domain.Enums.ReverseDropStatus.Sent)
        {
            throw new System.InvalidOperationException($"Cannot decline a Reverse Drop with status {Status}.");
        }
        Status = JobDrop.Domain.Enums.ReverseDropStatus.Declined;
    }

    public void Expire()
    {
        Status = JobDrop.Domain.Enums.ReverseDropStatus.ExpiredRefunded;
    }
}

