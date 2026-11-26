using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Services;
using Xunit;

namespace JobDrop.IntegrationTests.Features;

/// <summary>
/// Tests atomic wallet operations under high concurrency.
/// Ensures exactly 1 succeeds when 20 compete for last credit.
/// </summary>
public sealed class DropWalletConcurrencyTests
{
    [Fact]
    public void ConcurrentConsumption_LastCredit_OnlyOneSucceeds()
    {
        // Arrange
        var wallet = new DropWalletBalance(Guid.NewGuid(), dailyLimit: 1);
        var policy = new DropWalletPolicy();
        var results = new bool[20];

        // Act - 20 concurrent attempts
        Parallel.For(0, 20, i =>
        {
            try
            {
                policy.ConsumeCredit(wallet);
                results[i] = true;
            }
            catch (InvalidOperationException)
            {
                results[i] = false;
            }
        });

        // Assert - exactly 1 succeeded
        Assert.Equal(1, results.Count(r => r));
        Assert.Equal(0, wallet.DailyRemaining);
    }

    [Fact]
    public void Refund_AfterConsumption_RestoresBalance()
    {
        // Arrange
        var wallet = new DropWalletBalance(Guid.NewGuid(), dailyLimit: 5);
        var policy = new DropWalletPolicy();

        // Act
        var bucket = policy.ConsumeCredit(wallet);
        policy.RefundCredit(wallet, bucket);

        // Assert
        Assert.Equal(5, wallet.DailyRemaining);
    }
}