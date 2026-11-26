using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Messaging;
using JobDrop.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Services;

public class ConversationService
{
    private readonly IApplicationDbContext _context;

    public ConversationService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Conversation> CreatePendingInvitationAsync(
        Guid companyId,
        Guid candidateId,
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var conversation = new Conversation(companyId, candidateId, jobId)
        {
            State = ConversationState.PendingInvitation,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddHours(48),
            MessagingEnabled = false
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(cancellationToken);

        return conversation;
    }

    public async Task<bool> AcceptInvitationAsync(
        Guid conversationId,
        Guid candidateId,
        CancellationToken cancellationToken = default)
    {
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId && c.CandidateId == candidateId, cancellationToken);

        if (conversation == null || conversation.State != ConversationState.PendingInvitation)
        {
            return false;
        }

        if (conversation.ExpiresAtUtc.HasValue && DateTimeOffset.UtcNow > conversation.ExpiresAtUtc.Value)
        {
            conversation.State = ConversationState.Expired;
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        conversation.State = ConversationState.Active;
        conversation.AcceptedAtUtc = DateTimeOffset.UtcNow;
        conversation.MessagingEnabled = true;
        conversation.IsExclusivityActive = true;
        conversation.ExclusivityExpiresAtUtc = DateTimeOffset.UtcNow.AddHours(48);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeclineInvitationAsync(
        Guid conversationId,
        Guid candidateId,
        CancellationToken cancellationToken = default)
    {
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId && c.CandidateId == candidateId, cancellationToken);

        if (conversation == null || conversation.State != ConversationState.PendingInvitation)
        {
            return false;
        }

        conversation.State = ConversationState.Closed;
        conversation.DeclinedAtUtc = DateTimeOffset.UtcNow;
        conversation.MessagingEnabled = false;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> IsMessagingEnabledAsync(
        Guid conversationId,
        CancellationToken cancellationToken = default)
    {
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId, cancellationToken);

        return conversation?.MessagingEnabled ?? false;
    }
}
