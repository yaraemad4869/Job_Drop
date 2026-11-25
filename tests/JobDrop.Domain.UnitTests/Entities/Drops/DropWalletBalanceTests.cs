using FluentAssertions;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Enums;
using Xunit;

namespace JobDrop.Domain.UnitTests.Entities.Drops;

public class DropWalletBalanceTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDailyLimit()
    {
        var wallet = new DropWalletBalance(Guid.NewGuid(), 5);
        wallet.DailyRemaining.Should().Be(5);
        wallet.DailyLimit.Should().Be(5);
        wallet.BonusBalance.Should().Be(0);
        wallet.SpecialBalance.Should().Be(0);
    }

    [Fact]
    public void TryConsumeCredit_ShouldConsumeFromDailyFirst()
    {
        var wallet = new DropWalletBalance(Guid.NewGuid(), 3);
        wallet.TryConsumeCredit(out var bucket).Should().BeTrue();
        bucket.Should().Be(WalletBucket.Daily);
        wallet.DailyRemaining.Should().Be(2);
    }

    [Fact]
    public void TryConsumeCredit_ShouldFallbackToBonusWhenDailyIsEmpty()
    {
        var wallet = new DropWalletBalance(Guid.NewGuid(), 0);
        // Manually add bonus for test (simulating top-up)
        wallet.GetType().GetProperty("BonusBalance")?.SetValue(wallet, 2);
        
        wallet.TryConsumeCredit(out var bucket).Should().BeTrue();
        bucket.Should().Be(WalletBucket.Bonus);
        wallet.BonusBalance.Should().Be(1);
    }

    [Fact]
    public void TryConsumeCredit_ShouldFallbackToSpecialWhenDailyAndBonusAreEmpty()
    {
        var wallet = new DropWalletBalance(Guid.NewGuid(), 0);
        wallet.GetType().GetProperty("SpecialBalance")?.SetValue(wallet, 1);
        
        wallet.TryConsumeCredit(out var bucket).Should().BeTrue();
        bucket.Should().Be(WalletBucket.Special);
        wallet.SpecialBalance.Should().Be(0);
    }

    [Fact]
    public void TryConsumeCredit_ShouldReturnFalseWhenAllBucketsAreEmpty()
    {
        var wallet = new DropWalletBalance(Guid.NewGuid(), 0);
        wallet.TryConsumeCredit(out _).Should().BeFalse();
    }

    [Fact]
    public void RefundCredit_ShouldAddBackToCorrectBucket()
    {
        var wallet = new DropWalletBalance(Guid.NewGuid(), 2);
        wallet.TryConsumeCredit(out _); // Daily becomes 1
        
        wallet.RefundCredit(WalletBucket.Daily);
        wallet.DailyRemaining.Should().Be(2);
        
        wallet.RefundCredit(WalletBucket.Bonus);
        wallet.BonusBalance.Should().Be(1);
    }
}
