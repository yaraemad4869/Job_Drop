namespace JobDrop.Domain.Entities.Drops;

/// <summary>
/// BE-06: Distinct action log for interviews, offers, invitations, rejections, shortlists.
/// Each action has separate authorization, valid source states, SLA effects, and credit rules.
/// </summary>
public class ApplicationActionLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ApplicationId { get; set; }
    public string ActionType { get; set; } = string.Empty; // InterviewInvite, OfferExtended, OtherJobInvite, Rejected, Shortlisted
    public Guid PerformedByUserId { get; set; }
    public string? SourceState { get; set; }
    public string? TargetState { get; set; }
    public bool ConsumesCredit { get; set; } = false;
    public decimal? CreditAmount { get; set; }
    public DateTimeOffset PerformedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string? MetadataJson { get; set; }
}
