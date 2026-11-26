using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Messaging;
using JobDrop.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.ReverseDrops.Handlers;

public sealed class RespondToReverseDropCommandHandler
    : IRequestHandler<Commands.RespondToReverseDropCommand, bool>
{
    private readonly IApplicationDbContext _db;

    public RespondToReverseDropCommandHandler(IApplicationDbContext db) => _db = db;

    public async Task<bool> Handle(
        Commands.RespondToReverseDropCommand request,
        CancellationToken cancellationToken)
    {
        var reverseDrop = await _db.ReverseDrops
            .FirstOrDefaultAsync(rd => rd.Id == request.ReverseDropId, cancellationToken)
            ?? throw new InvalidOperationException("Reverse Drop not found");

        if (reverseDrop.Status != ReverseDropStatus.Sent)
        {
            throw new InvalidOperationException($"Reverse Drop already {reverseDrop.Status}");
        }

        if (request.Accept)
        {
            reverseDrop.Accept();

            // Create conversation using default constructor
            var conversation = new Conversation();
            conversation.ReverseDropId = reverseDrop.Id;
            conversation.CompanyId = reverseDrop.CompanyId;
            conversation.CandidateProfileId = reverseDrop.CandidateProfileId;

            _db.Conversations.Add(conversation);
        }
        else
        {
            reverseDrop.Decline();

            var pool = await _db.ReverseDropPools
                .Where(p => p.CompanyId == reverseDrop.CompanyId)
                .OrderByDescending(p => p.CycleStartUtc)
                .FirstOrDefaultAsync(cancellationToken);

            pool?.RefundCredit();
        }

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}