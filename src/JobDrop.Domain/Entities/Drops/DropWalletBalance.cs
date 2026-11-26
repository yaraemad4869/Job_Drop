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
    public byte[] RowVersion { get; private set; } = null!;

    public void ResetDaily(DateTime nowUtc)
    {
        DailyRemaining = DailyLimit;
        DailyResetAtUtc = nowUtc.Date.AddDays(1);
    }

    public void DebitSpecial(int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (SpecialBalance < amount) throw new InvalidOperationException("Insufficient Special balance");
        SpecialBalance -= amount;
    }

    public void CreditSpecial(int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        SpecialBalance += amount;
    }

    public void DebitDaily()
    {
        if (DailyRemaining <= 0) throw new InvalidOperationException("No daily credits remaining");
        DailyRemaining--;
    }

    public void DebitBonus()
    {
        if (BonusBalance <= 0) throw new InvalidOperationException("No bonus credits remaining");
        BonusBalance--;
    }


    /// <summary>
    /// Test factory method - creates instance with all properties settable for testing.
    /// </summary>
    public static DropWalletBalance CreateForTest(
        Guid candidateProfileId,
        int dailyLimit = 5,
        int dailyRemaining = 5,
        int bonusBalance = 0,
        int specialBalance = 0,
        byte[]? rowVersion = null)
    {
        var wallet = new DropWalletBalance(candidateProfileId, dailyLimit);
        wallet.DailyRemaining = dailyRemaining;
        wallet.BonusBalance = bonusBalance;
        wallet.SpecialBalance = specialBalance;
        if (rowVersion != null) wallet.RowVersion = rowVersion;
        return wallet;
    }
}

