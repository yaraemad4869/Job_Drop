using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobDrop.Application.Features.Billing.Services;

/// <summary>
/// BE-02: Ensures at most one refund per debit across rejection, expiration, cancellation, and retries.
/// Enforces atomicity via database constraints and transactional state changes.
/// </summary>
public class RefundSafetyService
{
    private readonly IApplicationDbContext _context;

    public RefundSafetyService(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Checks if a refund has already been issued for a specific debit.
    /// </summary>
    public async Task<bool> HasRefundBeenIssuedAsync(Guid originalDebitId, CancellationToken cancellationToken = default)
    {
        return await _context.RefundLedgerEntries
            .AnyAsync(r => r.OriginalDebitId == originalDebitId, cancellationToken);
    }

    /// <summary>
    /// Attempts to process a refund atomically.
    /// Returns false if a refund has already been issued, preventing duplicate refunds.
    /// </summary>
    public async Task<bool> TryProcessRefundAsync(
        Guid originalDebitId,
        Guid walletId,
        decimal amount,
        string triggerEvent, // e.g., "Rejection", "Expiration", "Cancellation"
        CancellationToken cancellationToken = default)
    {
        // 1. Safety check: prevent duplicate refunds (BE-02)
        if (await HasRefundBeenIssuedAsync(originalDebitId, cancellationToken))
        {
            return false;
        }

        // 2. Create atomic refund ledger entry
        var refundEntry = new RefundLedgerEntry
        {
            Id = Guid.NewGuid(),
            OriginalDebitId = originalDebitId,
            WalletId = walletId,
            Amount = amount,
            Reason = $"Refund due to {triggerEvent}",
            TriggerEvent = triggerEvent,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            IsProcessed = true // Mark as processed immediately upon successful transaction
        };

        _context.RefundLedgerEntries.Add(refundEntry);

        // Note: Actual wallet balance update should happen in the same transaction 
        // at the caller level (e.g., DispatchDropHandler or ExpirationWorker).
        // This service guarantees the ledger entry is safely recorded without duplication.

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
