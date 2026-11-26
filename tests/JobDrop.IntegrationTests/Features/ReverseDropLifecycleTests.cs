using JobDrop.Domain.Entities.ReverseDrops;
using JobDrop.Domain.Enums;
using Xunit;

namespace JobDrop.IntegrationTests.Features;

/// <summary>
/// Tests Reverse Drop lifecycle:
/// - Dispatch consumes pool credit
/// - Accept creates conversation
/// - Decline refunds credit
/// - 48h expiry auto-refunds
/// </summary>
public sealed class ReverseDropLifecycleTests
{
    [Fact]
    public void Dispatch_ConsumesPoolCredit()
    {
        // Arrange - use default constructor + property setters
        var pool = new ReverseDropPool();
        pool.CompanyId = Guid.NewGuid();
        pool.CycleStartUtc = DateTime.UtcNow;
        pool.CycleEndUtc = DateTime.UtcNow.AddDays(30);
        pool.MonthlyRemaining = 10;
        pool.TopUpRemaining = 0;

        // Act
        var consumed = pool.TryConsumeCredit();

        // Assert
        Assert.True(consumed);
        Assert.Equal(9, pool.MonthlyRemaining);
    }

    [Fact]
    public void Decline_RefundsCreditToPool()
    {
        // Arrange
        var pool = new ReverseDropPool();
        pool.CompanyId = Guid.NewGuid();
        pool.CycleStartUtc = DateTime.UtcNow;
        pool.CycleEndUtc = DateTime.UtcNow.AddDays(30);
        pool.MonthlyRemaining = 10;
        pool.TopUpRemaining = 0;
        pool.TryConsumeCredit(); // Now 9

        // Act
        pool.RefundCredit();

        // Assert
        Assert.Equal(10, pool.MonthlyRemaining);
    }

    [Fact]
    public void Accept_TransitionsToAccepted()
    {
        // Arrange - use default constructor + property setters
        var reverseDrop = new ReverseDrop();
        reverseDrop.CompanyId = Guid.NewGuid();
        reverseDrop.JobId = Guid.NewGuid();
        reverseDrop.CandidateProfileId = Guid.NewGuid();
        reverseDrop.DispatchNote = "Test";
        reverseDrop.ExpiresAtUtc = DateTimeOffset.UtcNow.AddHours(48);
        reverseDrop.IdempotencyKey = "test-key";
        reverseDrop.Status = ReverseDropStatus.Sent;

        // Act
        reverseDrop.Accept();

        // Assert
        Assert.Equal(ReverseDropStatus.Accepted, reverseDrop.Status);
    }

    [Fact]
    public void ExpireAfter48h_AutoRefunds()
    {
        // Arrange
        var reverseDrop = new ReverseDrop();
        reverseDrop.CompanyId = Guid.NewGuid();
        reverseDrop.JobId = Guid.NewGuid();
        reverseDrop.CandidateProfileId = Guid.NewGuid();
        reverseDrop.DispatchNote = "Test";
        reverseDrop.ExpiresAtUtc = DateTimeOffset.UtcNow.AddHours(-1); // Already expired
        reverseDrop.IdempotencyKey = "test-key";
        reverseDrop.Status = ReverseDropStatus.Sent;

        // Act
        reverseDrop.Expire();

        // Assert
        Assert.Equal(ReverseDropStatus.ExpiredRefunded, reverseDrop.Status);
    }

    [Fact]
    public void PoolExhausted_TryConsumeCredit_ReturnsFalse()
    {
        // Arrange
        var pool = new ReverseDropPool();
        pool.CompanyId = Guid.NewGuid();
        pool.CycleStartUtc = DateTime.UtcNow;
        pool.CycleEndUtc = DateTime.UtcNow.AddDays(30);
        pool.MonthlyRemaining = 0;
        pool.TopUpRemaining = 0;

        // Act
        var consumed = pool.TryConsumeCredit();

        // Assert
        Assert.False(consumed);
    }

    [Fact]
    public void PoolWithTopUp_UsesTopUpAfterMonthlyExhausted()
    {
        // Arrange
        var pool = new ReverseDropPool();
        pool.CompanyId = Guid.NewGuid();
        pool.CycleStartUtc = DateTime.UtcNow;
        pool.CycleEndUtc = DateTime.UtcNow.AddDays(30);
        pool.MonthlyRemaining = 0;
        pool.TopUpRemaining = 5;

        // Act
        var consumed = pool.TryConsumeCredit();

        // Assert
        Assert.True(consumed);
        Assert.Equal(4, pool.TopUpRemaining);
    }

    [Fact]
    public void Decline_WhenNotSent_ThrowsException()
    {
        // Arrange
        var reverseDrop = new ReverseDrop();
        reverseDrop.CompanyId = Guid.NewGuid();
        reverseDrop.JobId = Guid.NewGuid();
        reverseDrop.CandidateProfileId = Guid.NewGuid();
        reverseDrop.DispatchNote = "Test";
        reverseDrop.ExpiresAtUtc = DateTimeOffset.UtcNow.AddHours(48);
        reverseDrop.IdempotencyKey = "test-key";
        reverseDrop.Status = ReverseDropStatus.Accepted; // Already accepted

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => reverseDrop.Decline());
    }
}