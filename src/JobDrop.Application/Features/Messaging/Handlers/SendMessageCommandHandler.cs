using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Messaging.Handlers;

public sealed class SendMessageCommandHandler
    : IRequestHandler<Commands.SendMessageCommand, Commands.SendMessageResult>
{
    private readonly IApplicationDbContext _db;

    public SendMessageCommandHandler(IApplicationDbContext db) => _db = db;

    public async Task<Commands.SendMessageResult> Handle(
        Commands.SendMessageCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _db.Conversations
            .FirstOrDefaultAsync(c => c.Id == request.ConversationId, cancellationToken)
            ?? throw new InvalidOperationException("Conversation not found");

        var message = new Message();
        message.ConversationId = request.ConversationId;
        message.SenderUserId = Guid.Empty; // TODO: From ICurrentUser
        message.Body = request.Body;
        if (request.AttachmentId.HasValue)
        {
            message.AttachmentKey = request.AttachmentId.Value.ToString();
        }

        _db.Messages.Add(message);
        conversation.LastMessageAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        return new Commands.SendMessageResult(message.Id);
    }
}