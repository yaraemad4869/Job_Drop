using MediatR;
using JobDrop.Application.Services;

namespace JobDrop.Application.Features.ConversationFlow;

public class ConversationHandler : 
    IRequestHandler<AcceptConversationCommand, ConversationResult>,
    IRequestHandler<DeclineConversationCommand, ConversationResult>
{
    private readonly ConversationService _conversationService;

    public ConversationHandler(ConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    public async Task<ConversationResult> Handle(AcceptConversationCommand request, CancellationToken cancellationToken)
    {
        // BE-07: Delegate to service with atomic acceptance
        var success = await _conversationService.AcceptInvitationAsync(request.ConversationId, Guid.Empty, cancellationToken);
        return new ConversationResult(success, success ? "Conversation accepted" : "Acceptance failed");
    }

    public async Task<ConversationResult> Handle(DeclineConversationCommand request, CancellationToken cancellationToken)
    {
        // BE-07: Delegate to service with atomic decline
        var success = await _conversationService.DeclineInvitationAsync(request.ConversationId, Guid.Empty, cancellationToken);
        return new ConversationResult(success, success ? "Conversation declined" : "Decline failed");
    }
}
