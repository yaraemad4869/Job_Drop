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

    /// <summary>
    /// Factory method to create a new Application with Id and required fields.
    /// </summary>
    public static Application Create(
        Guid id,
        Guid jobId,
        Guid companyId,
        Guid candidateProfileId,
        string? idempotencyKey = null)
    {
        var app = new Application();
        app.Id = id;
        app.JobId = jobId;
        app.CompanyId = companyId;
        app.CandidateProfileId = candidateProfileId;
        app.Status = JobDrop.Domain.Enums.DropStatus.Queued;
        app.IdempotencyKey = idempotencyKey;
        return app;
    }

    public void Shortlist()
    {
        if (Status != JobDrop.Domain.Enums.DropStatus.Delivered && Status != JobDrop.Domain.Enums.DropStatus.Opened && Status != JobDrop.Domain.Enums.DropStatus.Reviewed)
            throw new InvalidOperationException("Cannot shortlist from status " + Status);
        Status = JobDrop.Domain.Enums.DropStatus.Shortlisted;
    }

    public void ScheduleInterview()
    {
        if (Status != JobDrop.Domain.Enums.DropStatus.Shortlisted && Status != JobDrop.Domain.Enums.DropStatus.Delivered && Status != JobDrop.Domain.Enums.DropStatus.Opened)
            throw new InvalidOperationException("Cannot schedule interview from status " + Status);
        Status = JobDrop.Domain.Enums.DropStatus.InterviewScheduled;
    }

    public void ExtendOffer()
    {
        if (Status != JobDrop.Domain.Enums.DropStatus.InterviewScheduled && Status != JobDrop.Domain.Enums.DropStatus.Shortlisted)
            throw new InvalidOperationException("Cannot extend offer from status " + Status);
        Status = JobDrop.Domain.Enums.DropStatus.OfferExtended;
    }

    public void RejectWithReason(string reasonCode, string? note = null)
    {
        if (string.IsNullOrWhiteSpace(reasonCode))
            throw new ArgumentException("Rejection reason code is mandatory", nameof(reasonCode));
        Status = JobDrop.Domain.Enums.DropStatus.Rejected;
        RejectionReasonCode = reasonCode;
        RejectionNote = note;
        FinalizedAtUtc = System.DateTimeOffset.UtcNow;
    }

    // BE-01: SLA tracking fields
    public DateTimeOffset? SentAtUtc { get; set; }
    public DateTimeOffset? DueAtUtc { get; set; }
    public string SlaPolicyVersion { get; set; } = "v1";
    public Guid? SlaPolicyId { get; set; }
    
    // BE-18: Queue tracking
    public DateTimeOffset? QueuedAtUtc { get; set; }
    public bool IsQueued { get; set; } = false;
    
    // BE-12: Review tracking
    public DateTimeOffset? ReviewedAtUtc { get; set; }
    public Guid? ReviewedByUserId { get; set; }
}

