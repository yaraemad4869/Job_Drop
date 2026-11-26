namespace JobDrop.Domain.Entities.Messaging;

/// <summary>
/// BE-08: Interview proposal with multiple slots and time zones.
/// Supports Voice/Video, explicit acceptance flow.
/// </summary>
public class InterviewProposalState
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProposalId { get; set; }
    public string ProposalType { get; set; } = string.Empty; // Voice, Video
    public string ProposedSlotsJson { get; set; } = string.Empty; // Array of DateTimeOffset slots
    public string TimeZone { get; set; } = "UTC";
    public string Status { get; set; } = "Pending"; // Pending, Accepted, Declined, Expired
    public Guid? ConfirmedSlotId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? RespondedAtUtc { get; set; }
}
