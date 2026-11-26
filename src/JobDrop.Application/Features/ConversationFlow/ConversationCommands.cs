using MediatR;

namespace JobDrop.Application.Features.ConversationFlow;

public record AcceptConversationCommand(Guid ConversationId) : IRequest<ConversationResult>;
public record DeclineConversationCommand(Guid ConversationId) : IRequest<ConversationResult>;

public record ConversationResult(bool Success, string Message);

