namespace JobDrop.Application.DTOs.Requests;

public record CreateInterviewProposalRequest(
    Guid ConversationId,
    string ProposalType,
    List<DateTimeOffset> ProposedSlots,
    string TimeZone = "UTC");
