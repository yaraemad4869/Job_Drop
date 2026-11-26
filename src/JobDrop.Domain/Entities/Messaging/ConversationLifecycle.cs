using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Messaging;

/// <summary>
/// BE-07: Conversation lifecycle with atomic state transitions.
/// Dispatch creates pending invitation; acceptance enables messaging.
/// </summary>
public class ConversationLifecycle
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ConversationId { get; set; }
    public ConversationState State { get; set; } = ConversationState.PendingInvitation;
    public Guid InitiatorUserId { get; set; }
    public Guid? RespondentUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? AcceptedAtUtc { get; set; }
    public DateTimeOffset? ExpiresAtUtc { get; set; }
    public bool IsExclusivityActive { get; set; } = false;
    public DateTimeOffset? ExclusivityExpiresAtUtc { get; set; }
    
    public bool CanAcceptMessage => 
        State == ConversationState.Active && AcceptedAtUtc.HasValue;
}
