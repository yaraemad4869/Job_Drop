using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.RecruiterReview.Commands;
using JobDrop.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.RecruiterReview.Handlers;

public sealed class RejectDropHandler : IRequestHandler<RejectDropCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly DropWalletPolicy _walletPolicy;
    public RejectDropHandler(IApplicationDbContext context, DropWalletPolicy walletPolicy)
    {
        _context = context;
        _walletPolicy = walletPolicy;
    }

    public async Task<bool> Handle(RejectDropCommand request, CancellationToken ct)
    {
        var app = await _context.Applications.FirstOrDefaultAsync(a => a.Id == request.DropId, ct)
            ?? throw new JobDrop.Domain.Exceptions.NotFoundException("Drop not found");
        
        app.RejectWithReason(request.ReasonCode, request.Note);
        
        // Instant refund of 1 Bonus credit to candidate (per spec)
        var wallet = await _context.DropWalletBalances
            .FirstOrDefaultAsync(w => w.CandidateProfileId == app.CandidateProfileId, ct);
        if (wallet != null)
        {
            _walletPolicy.RefundCredit(wallet, JobDrop.Domain.Enums.WalletBucket.Bonus);
            
            var ledger = new global::JobDrop.Domain.Entities.Drops.DropLedgerEntry(
                candidateProfileId: app.CandidateProfileId,
                bucket: JobDrop.Domain.Enums.WalletBucket.Bonus,
                delta: 1,
                reasonCode: $"refund:reject:{app.Id}",
                referenceId: app.Id.ToString(),
                idempotencyKey: $"reject_refund_{app.Id}_{System.Guid.NewGuid()}");
            _context.DropLedgerEntries.Add(ledger);
        }
        
        await _context.SaveChangesAsync(ct);
        return true;
    }
}