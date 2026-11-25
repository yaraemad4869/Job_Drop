using MediatR;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Drops.Commands;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Drops.Commands;

public sealed class DispatchDropHandler : IRequestHandler<DispatchDropCommand, DispatchDropResult>
{
    private readonly IApplicationDbContext _context;

    public DispatchDropHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DispatchDropResult> Handle(DispatchDropCommand request, CancellationToken cancellationToken)
    {
        // 1. Idempotency Check
        var existingLedger = await _context.DropLedgerEntries
            .FirstOrDefaultAsync(x => x.IdempotencyKey == request.IdempotencyKey, cancellationToken);
            
        if (existingLedger != null)
        {
            throw new DomainRuleException("Drop already dispatched with this idempotency key.", "IDEMPOTENCY_CONFLICT");
        }

        // 2. Fetch Wallet and Job concurrently
        var wallet = await _context.DropWalletBalances
            .FirstOrDefaultAsync(x => x.CandidateProfileId == request.CandidateProfileId, cancellationToken)
            ?? throw new NotFoundException("Wallet not found for candidate.");

        var job = await _context.Jobs
            .FirstOrDefaultAsync(x => x.Id == request.JobId, cancellationToken)
            ?? throw new NotFoundException("Job not found.");

        // 3. Atomic Intake Cap Check
        if (!job.TryIncrementIntake())
        {
            throw new DomainRuleException("Job has reached its daily intake capacity.", "INTAKE_CAP_REACHED");
        }

        // 4. Attempt to Consume Credit
        if (!wallet.TryConsumeCredit(out var consumedFrom))
        {
            // Rollback intake increment if wallet is empty
            job.DecrementIntake();
            throw new DomainRuleException("Insufficient Drop credits.", "INSUFFICIENT_BALANCE");
        }

        // 5. Create Application & SLA
        var applicationId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var sla = new ApplicationSla(applicationId, now, 48);

        // 6. Create Ledger Entry
        var ledgerEntry = new DropLedgerEntry(
            request.CandidateProfileId,
            consumedFrom,
            -1,
            "DROP_DISPATCH",
            applicationId.ToString(),
            request.IdempotencyKey);

        _context.DropWalletBalances.Update(wallet);
        _context.Jobs.Update(job); // Save the incremented intake
        _context.ApplicationSlas.Add(sla);
        _context.DropLedgerEntries.Add(ledgerEntry);

        await _context.SaveChangesAsync(cancellationToken);

        return new DispatchDropResult(applicationId, consumedFrom, sla.DueAtUtc, job.CurrentApplicantsCount);
    }
}
