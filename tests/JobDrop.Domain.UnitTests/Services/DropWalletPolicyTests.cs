using Xunit;
using FluentAssertions;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Exceptions;
using JobDrop.Domain.Services;

namespace JobDrop.Domain.UnitTests.Services;

public class DropWalletPolicyTests
{
    private readonly DropWalletPolicy _policy = new();

    [Fact]
    public void Consume_DailyAvailable_ShouldConsumeFromDaily()
    {
        var wallet = CreateWallet(dailyRemaining: 3, bonus: 0, special: 0);
        var bucket = _policy.Consume(wallet, "test-key");
        bucket.Should().Be(WalletBucket.Daily);
        wallet.DailyRemaining.Should().Be(2);
    }

    [Fact]
    public void Consume_DailyEmpty_BonusAvailable_ShouldConsumeFromBonus()
    {
        var wallet = CreateWallet(dailyRemaining: 0, bonus: 5, special: 0);
        var bucket = _policy.Consume(wallet, "test-key");
        bucket.Should().Be(WalletBucket.Bonus);
        wallet.BonusBalance.Should().Be(4);
    }

    [Fact]
    public void Consume_DailyAndBonusEmpty_SpecialAvailable_ShouldConsumeFromSpecial()
    {
        var wallet = CreateWallet(dailyRemaining: 0, bonus: 0, special: 2);
        var bucket = _policy.Consume(wallet, "test-key");
        bucket.Should().Be(WalletBucket.Special);
        wallet.SpecialBalance.Should().Be(1);
    }

    [Fact]
    public void Consume_AllBucketsEmpty_ShouldThrowInsufficientBalance()
    {
        var wallet = CreateWallet(dailyRemaining: 0, bonus: 0, special: 0);
        var act = () => _policy.Consume(wallet, "test-key");
        act.Should().Throw<InsufficientBalanceException>();
    }

    [Fact]
    public void Consume_PriorityOrder_ShouldFollowDaily_Bonus_Special()
    {
        var wallet = CreateWallet(dailyRemaining: 1, bonus: 1, special: 1);
        _policy.Consume(wallet, "key1").Should().Be(WalletBucket.Daily);
        _policy.Consume(wallet, "key2").Should().Be(WalletBucket.Bonus);
        _policy.Consume(wallet, "key3").Should().Be(WalletBucket.Special);
    }

    [Fact]
    public void Refund_Daily_ShouldIncrementDailyRemaining()
    {
        var wallet = CreateWallet(dailyRemaining: 2, dailyLimit: 5, bonus: 0, special: 0);
        _policy.Refund(wallet, WalletBucket.Daily);
        wallet.DailyRemaining.Should().Be(3);
    }

    [Fact]
    public void Refund_Daily_AtLimit_ShouldNotExceedLimit()
    {
        var wallet = CreateWallet(dailyRemaining: 5, dailyLimit: 5, bonus: 0, special: 0);
        _policy.Refund(wallet, WalletBucket.Daily);
        wallet.DailyRemaining.Should().Be(5);
    }

    [Fact]
    public void Refund_Bonus_ShouldIncrementBonusBalance()
    {
        var wallet = CreateWallet(dailyRemaining: 0, bonus: 2, special: 0);
        _policy.Refund(wallet, WalletBucket.Bonus);
        wallet.BonusBalance.Should().Be(3);
    }

    [Fact]
    public void Refund_Special_ShouldIncrementSpecialBalance()
    {
        var wallet = CreateWallet(dailyRemaining: 0, bonus: 0, special: 2);
        _policy.Refund(wallet, WalletBucket.Special);
        wallet.SpecialBalance.Should().Be(3);
    }

    [Fact]
    public void ResetDailyIfNeeded_PastResetTime_ShouldReset()
    {
        var wallet = CreateWallet(dailyRemaining: 0, dailyLimit: 5);
        wallet.DailyResetAtUtc = DateTimeOffset.UtcNow.AddHours(-1);
        _policy.ResetDailyIfNeeded(wallet, DateTimeOffset.UtcNow);
        wallet.DailyRemaining.Should().Be(5);
    }

    [Fact]
    public void ResetDailyIfNeeded_BeforeResetTime_ShouldNotReset()
    {
        var wallet = CreateWallet(dailyRemaining: 2, dailyLimit: 5);
        wallet.DailyResetAtUtc = DateTimeOffset.UtcNow.AddHours(12);
        _policy.ResetDailyIfNeeded(wallet, DateTimeOffset.UtcNow);
        wallet.DailyRemaining.Should().Be(2);
    }

    private static DropWalletBalance CreateWallet(
        int dailyRemaining = 5, int dailyLimit = 5, int bonus = 0, int special = 0)
    {
        return new DropWalletBalance
        {
            CandidateProfileId = Guid.NewGuid(),
            DailyRemaining = dailyRemaining,
            DailyLimit = dailyLimit,
            DailyResetAtUtc = DateTimeOffset.UtcNow.Date.AddDays(1),
            BonusBalance = bonus,
            SpecialBalance = special
        };
    }
}