using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.ReverseDrops;
using JobDrop.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.ReverseDrops.Handlers;

public sealed class DispatchReverseDropCommandHandler
    : IRequestHandler<Commands.DispatchReverseDropCommand, Commands.DispatchReverseDropResult>
{
    private readonly IApplicationDbContext _db;

    public DispatchReverseDropCommandHandler(IApplicationDbContext db) => _db = db;

    public async Task<Commands.DispatchReverseDropResult> Handle(
        Commands.DispatchReverseDropCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _db.ReverseDrops
            .FirstOrDefaultAsync(rd => rd.IdempotencyKey == request.IdempotencyKey, cancellationToken);

        if (existing != null)
        {
            return new Commands.DispatchReverseDropResult(existing.Id, existing.ExpiresAtUtc.DateTime);
        }

        var cycleStart = DateTime.UtcNow.Date.AddDays(-DateTime.UtcNow.Day + 1);
        var pool = await _db.ReverseDropPools
            .FirstOrDefaultAsync(p => p.CompanyId == request.CompanyId && p.CycleStartUtc == cycleStart, cancellationToken);

        if (pool == null)
        {
            pool = new ReverseDropPool();
            pool.CompanyId = request.CompanyId;
            pool.CycleStartUtc = cycleStart;
            pool.CycleEndUtc = cycleStart.AddDays(30);
            pool.MonthlyRemaining = 30;
            pool.TopUpRemaining = 0;
            _db.ReverseDropPools.Add(pool);
        }

        if (!pool.TryConsumeCredit())
        {
            throw new InvalidOperationException("Reverse Drop pool exhausted.");
        }

        var reverseDrop = new ReverseDrop();
        reverseDrop.CompanyId = request.CompanyId;
        reverseDrop.JobId = request.JobId;
        reverseDrop.CandidateProfileId = request.CandidateProfileId;
        reverseDrop.DispatchNote = request.DispatchNote;
        // Fixed: Use DateTimeOffset for ExpiresAtUtc
        reverseDrop.ExpiresAtUtc = DateTimeOffset.UtcNow.AddHours(48);
        reverseDrop.IdempotencyKey = request.IdempotencyKey;
        reverseDrop.Status = ReverseDropStatus.Sent;

        if (request.OfferDocumentId.HasValue)
        {
            reverseDrop.OfferDocumentKey = request.OfferDocumentId.Value.ToString();
        }

        _db.ReverseDrops.Add(reverseDrop);
        await _db.SaveChangesAsync(cancellationToken);

        return new Commands.DispatchReverseDropResult(reverseDrop.Id, reverseDrop.ExpiresAtUtc.DateTime);
    }
}