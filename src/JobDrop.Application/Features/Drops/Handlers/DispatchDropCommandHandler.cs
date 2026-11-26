using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Drops.Commands;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Exceptions;
using JobDrop.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Drops.Handlers;

public sealed class DispatchDropCommandHandler : IRequestHandler<DispatchDropCommand, DispatchDropResult>
{
    private readonly IApplicationDbContext _context;
    private readonly DropWalletPolicy _walletPolicy;

    public DispatchDropCommandHandler(IApplicationDbContext context, DropWalletPolicy walletPolicy)
    {
        _context = context;
        _walletPolicy = walletPolicy;
    }

    public async Task<DispatchDropResult> Handle(DispatchDropCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _context.DropWalletBalances
            .FirstOrDefaultAsync(w => w.CandidateProfileId == request.CandidateProfileId, cancellationToken);

        if (wallet == null)
            throw new DomainRuleException("Wallet not found");

        // Use domain policy to consume credit (follows Daily -> Bonus -> Special order)
        var bucket = _walletPolicy.ConsumeCredit(wallet);

        await _context.SaveChangesAsync(cancellationToken);

        // Return result matching actual DispatchDropResult signature:
        // (Guid ApplicationId, WalletBucket ConsumedFrom, DateTime DueAtUtc, int BatchRank)
        return new DispatchDropResult(
            ApplicationId: Guid.NewGuid(),
            ConsumedFrom: bucket,
            DueAtUtc: DateTime.UtcNow.AddHours(48),
            BatchRank: 1
        );
    }
}