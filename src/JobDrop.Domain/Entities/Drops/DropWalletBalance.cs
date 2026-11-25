using JobDrop.Domain.Enums;
using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Drops;

public sealed class DropWalletBalance : BaseEntity
{
    public Guid CandidateProfileId { get; private set; }
    public int DailyRemaining { get; private set; }
    public int DailyLimit { get; private set; }
    public DateTime DailyResetAtUtc { get; private set; }
    public int BonusBalance { get; private set; }
    public int SpecialBalance { get; private set; }

    private DropWalletBalance() { }

    public DropWalletBalance(Guid candidateProfileId, int dailyLimit)
    {
        CandidateProfileId = candidateProfileId;
        DailyLimit = dailyLimit;
        DailyRemaining = dailyLimit;
        BonusBalance = 0;
        SpecialBalance = 0;
        DailyResetAtUtc = DateTime.UtcNow.Date.AddDays(1);
    }

    public bool TryConsumeCredit(out WalletBucket consumedFrom)
    {
        if (DailyRemaining > 0)
        {
            DailyRemaining--;
            consumedFrom = WalletBucket.Daily;
            return true;
        }
        if (BonusBalance > 0)
        {
            BonusBalance--;
            consumedFrom = WalletBucket.Bonus;
            return true;
        }
        if (SpecialBalance > 0)
        {
            SpecialBalance--;
            consumedFrom = WalletBucket.Special;
            return true;
        }
        
        consumedFrom = WalletBucket.Daily;
        return false;
    }

    public void RefundCredit(WalletBucket bucket)
    {
        if (bucket == WalletBucket.Daily) DailyRemaining++;
        else if (bucket == WalletBucket.Bonus) BonusBalance++;
        else if (bucket == WalletBucket.Special) SpecialBalance++;
    }
}