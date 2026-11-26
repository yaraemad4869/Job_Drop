namespace JobDrop.Domain.Enums;

/// <summary>
/// Conversation lifecycle states (BE-07).
/// Dispatch alone does not enable messaging; acceptance is required.
/// </summary>
public enum ConversationState
{
    PendingInvitation = 0,
    Active = 1,
    Closed = 2,
    Expired = 3
}
