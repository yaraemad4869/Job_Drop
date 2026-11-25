using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Messaging;

public sealed class InterviewProposal : BaseEntity
{
    public Guid ConversationId { get; set; }
    public Guid ProposedByUserId { get; set; }
    public CallType CallType { get; set; }
    public string SlotsJson { get; set; } = "[]";
    public DateTimeOffset? ConfirmedStartUtc { get; set; }
    public DateTimeOffset? ConfirmedEndUtc { get; set; }
    public string? MeetingUrl { get; set; }
    public InterviewStatus Status { get; set; } = InterviewStatus.Proposed;

    public Conversation? Conversation { get; set; }
}