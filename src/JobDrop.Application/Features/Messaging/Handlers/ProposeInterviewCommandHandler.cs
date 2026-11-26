using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Messaging;
using JobDrop.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Messaging.Handlers;

public sealed class ProposeInterviewCommandHandler
    : IRequestHandler<Commands.ProposeInterviewCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public ProposeInterviewCommandHandler(IApplicationDbContext db) => _db = db;

    public async Task<Guid> Handle(
        Commands.ProposeInterviewCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _db.Conversations
            .FirstOrDefaultAsync(c => c.Id == request.ConversationId, cancellationToken)
            ?? throw new InvalidOperationException("Conversation not found");

        var proposal = new InterviewProposal();
        proposal.ConversationId = request.ConversationId;
        proposal.ProposedByUserId = Guid.Empty; // TODO: From ICurrentUser
        // Fixed: Parse string to CallType enum
        proposal.CallType = Enum.TryParse<CallType>(request.CallType, true, out var callType) 
            ? callType 
            : CallType.Voice;
        proposal.SlotsJson = System.Text.Json.JsonSerializer.Serialize(request.Slots);

        _db.InterviewProposals.Add(proposal);
        await _db.SaveChangesAsync(cancellationToken);

        return proposal.Id;
    }
}