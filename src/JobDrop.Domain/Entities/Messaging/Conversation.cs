using JobDrop.Domain.Enums;
using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Messaging;

public sealed class Conversation : BaseEntity
{
    public Guid? ApplicationId { get; set; }
    public Guid? ReverseDropId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid CandidateProfileId { get; set; }
    public bool IsArchivedByCandidate { get; set; }
    public bool IsArchivedByCompany { get; set; }
    public DateTimeOffset LastMessageAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public Drops.Application? Application { get; set; }
    public ReverseDrops.ReverseDrop? ReverseDrop { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<InterviewProposal> InterviewProposals { get; set; } = new List<InterviewProposal>();

    // BE-07: Conversation lifecycle
    public DateTimeOffset? AcceptedAtUtc { get; set; }
    public DateTimeOffset? DeclinedAtUtc { get; set; }
    public DateTimeOffset? ExpiresAtUtc { get; set; }
    public bool IsExclusivityActive { get; set; } = false;
    public DateTimeOffset? ExclusivityExpiresAtUtc { get; set; }
    public bool MessagingEnabled { get; set; } = false;

    // BE-07: Additional required properties
    public Guid CandidateId { get; set; }
    public Guid JobId { get; set; }
    public ConversationState State { get; set; } = ConversationState.PendingInvitation;
    
    // Constructor for BE-07
    public Conversation(Guid companyId, Guid candidateId, Guid jobId)
    {
        CompanyId = companyId;
        CandidateId = candidateId;
        JobId = jobId;
        State = ConversationState.PendingInvitation;
    }
    
    // Parameterless constructor for EF Core
    public Conversation() { }
}


