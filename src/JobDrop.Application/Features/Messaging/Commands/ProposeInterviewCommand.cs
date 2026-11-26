using MediatR;

namespace JobDrop.Application.Features.Messaging.Commands;

public sealed record ProposeInterviewCommand(
    Guid ConversationId,
    string CallType,
    object[] Slots
) : IRequest<Guid>;