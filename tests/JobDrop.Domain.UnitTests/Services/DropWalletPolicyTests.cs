using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Services;
using Xunit;

namespace JobDrop.Domain.UnitTests.Services;

public sealed class DropWalletPolicyTests
{
    [Fact]
    public void ConsumeCredit_WithDailyRemaining_ConsumesFromDaily()
    {
        var wallet = new DropWalletBalance(Guid.NewGuid(), 5);
        var policy = new DropWalletPolicy();
        var bucket = policy.ConsumeCredit(wallet);
        Assert.Equal(WalletBucket.Daily, bucket);
    }

    [Fact]
    public void ConsumeCredit_WithNoCredits_Throws()
    {
        var wallet = new DropWalletBalance(Guid.NewGuid(), 0);
        var policy = new DropWalletPolicy();
        Assert.Throws<InvalidOperationException>(() => policy.ConsumeCredit(wallet));
    }

    [Fact]
    public void ConsumeCredit_20Concurrent_LastCredit_OnlyOneSucceeds()
    {
        // TODO: Concurrency test with Parallel.ForEach
    }
}