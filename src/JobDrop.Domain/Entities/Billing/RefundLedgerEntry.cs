namespace JobDrop.Domain.Entities.Billing;

/// <summary>
/// BE-02: Atomic refund ledger entry linked to original debit.
/// Enforces at most one refund per debit via unique constraint.
/// </summary>
public class RefundLedgerEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OriginalDebitId { get; set; }
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string TriggerEvent { get; set; } = string.Empty; // Rejection, Expiration, Cancellation
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public bool IsProcessed { get; set; } = false;
}
