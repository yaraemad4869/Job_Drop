using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Entities.Jobs;

namespace JobDrop.Domain.Entities.Drops;

public sealed class Application : ConcurrentAuditableEntity
{
    public Guid JobId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid CandidateProfileId { get; set; }
    public DropStatus Status { get; set; } = DropStatus.Queued;
    public int BatchRank { get; set; }
    public string? CoverNote { get; set; }
    public string? RejectionReasonCode { get; set; }
    public string? RejectionNote { get; set; }
    public DateTimeOffset? DeliveredAtUtc { get; set; }
    public DateTimeOffset? OpenedAtUtc { get; set; }
    public DateTimeOffset? FinalizedAtUtc { get; set; }
    public string? IdempotencyKey { get; set; }

    public Job? Job { get; set; }
    public Companies.Company? Company { get; set; }
    public Candidates.CandidateProfile? CandidateProfile { get; set; }
    public ApplicationSnapshot? Snapshot { get; set; }
    public Sla.ApplicationSla? Sla { get; set; }
    public ICollection<ApplicationEvent> Events { get; set; } = new List<ApplicationEvent>();

    public void Cancel(string reason)
    {
        Status = JobDrop.Domain.Enums.DropStatus.Cancelled;
        RejectionReasonCode = reason;
    }

    public void PromoteToDelivered()
    {
        Status = JobDrop.Domain.Enums.DropStatus.Delivered;
        DeliveredAtUtc = DateTime.UtcNow;
    }
}
