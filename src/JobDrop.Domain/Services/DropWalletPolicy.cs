using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Services;

/// <summary>
/// Pure domain logic for Drop Wallet credit consumption/refund.
/// Consumption order: Daily -> Bonus -> Special
/// Enforces atomicity via optimistic concurrency (RowVersion).
/// </summary>
public sealed class DropWalletPolicy
{
    /// <summary>
    /// Consumes 1 credit from the wallet following priority order.
    /// Returns the bucket from which credit was consumed.
    /// Throws if no credits available.
    /// </summary>
    public WalletBucket ConsumeCredit(DropWalletBalance wallet)
    {
        if (wallet is null) throw new ArgumentNullException(nameof(wallet));

        if (!wallet.TryConsumeCredit(out var bucket))
        {
            throw new InvalidOperationException("Insufficient Drop credits. All buckets exhausted.");
        }

        return bucket;
    }

    /// <summary>
    /// Refunds 1 credit to the specified bucket.
    /// Used for SLA expiration refunds and queue cancellations.
    /// </summary>
    public void RefundCredit(DropWalletBalance wallet, WalletBucket bucket)
    {
        if (wallet is null) throw new ArgumentNullException(nameof(wallet));
        wallet.RefundCredit(bucket);
    }

    /// <summary>
    /// Resets daily allowance if reset time has passed.
    /// Called at start of each dispatch operation.
    /// </summary>
    public void EnsureDailyReset(DropWalletBalance wallet, DateTime nowUtc)
    {
        if (nowUtc >= wallet.DailyResetAtUtc)
        {
            wallet.ResetDaily(nowUtc);
        }
    }
}