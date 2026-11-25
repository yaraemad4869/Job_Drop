using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Exceptions;

namespace JobDrop.Domain.Services;

public sealed class DropWalletPolicy
{
    public const int DefaultDailyLimit = 5;

    public WalletBucket Consume(DropWalletBalance wallet, string idempotencyKey)
    {
        if (wallet.DailyRemaining > 0)
        {
            wallet.DailyRemaining--;
            return WalletBucket.Daily;
        }
        if (wallet.BonusBalance > 0)
        {
            wallet.BonusBalance--;
            return WalletBucket.Bonus;
        }
        if (wallet.SpecialBalance > 0)
        {
            wallet.SpecialBalance--;
            return WalletBucket.Special;
        }
        throw new InsufficientBalanceException();
    }

    public void Refund(DropWalletBalance wallet, WalletBucket bucket)
    {
        switch (bucket)
        {
            case WalletBucket.Daily:
                wallet.DailyRemaining = Math.Min(wallet.DailyRemaining + 1, wallet.DailyLimit);
                break;
            case WalletBucket.Bonus:
                wallet.BonusBalance++;
                break;
            case WalletBucket.Special:
                wallet.SpecialBalance++;
                break;
        }
    }

    public void ResetDailyIfNeeded(DropWalletBalance wallet, DateTimeOffset nowUtc)
    {
        if (nowUtc >= wallet.DailyResetAtUtc)
        {
            wallet.DailyRemaining = wallet.DailyLimit;
            wallet.DailyResetAtUtc = nowUtc.Date.AddDays(1);
        }
    }
}