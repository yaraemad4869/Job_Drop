using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Messaging;

public sealed class Message : BaseEntity
{
    public Guid ConversationId { get; set; }
    public Guid SenderUserId { get; set; }
    public string Body { get; set; } = string.Empty;
    public string? AttachmentKey { get; set; }
    public DateTimeOffset? ReadAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public Conversation? Conversation { get; set; }
}