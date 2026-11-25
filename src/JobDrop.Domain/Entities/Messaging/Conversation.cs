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
}