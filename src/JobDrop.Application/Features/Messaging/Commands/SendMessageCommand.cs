using MediatR;

namespace JobDrop.Application.Features.Messaging.Commands;

public sealed record SendMessageCommand(
    Guid ConversationId,
    string Body,
    Guid? AttachmentId
) : IRequest<SendMessageResult>;

public sealed record SendMessageResult(Guid MessageId);