using JobDrop.Infrastructure.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using JobDrop.Application.Features.Drops.Commands;
using JobDrop.Application.Features.Drops.Handlers;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Exceptions;
using JobDrop.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Drops.Commands;

public sealed class DispatchDropHandlerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public DispatchDropHandlerTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldDebitWalletAndCreateSla()
    {
        // Arrange - Use factory method to respect encapsulation
        var candidateId = Guid.NewGuid();
        var jobId = Guid.NewGuid();
        var rowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 };

        using var context = new ApplicationDbContext(_options);
        var wallet = DropWalletBalance.CreateForTest(
            candidateProfileId: candidateId,
            dailyLimit: 5,
            dailyRemaining: 5,
            rowVersion: rowVersion);
        
        context.DropWalletBalances.Add(wallet);
        await context.SaveChangesAsync();

        var command = new DispatchDropCommand(jobId, candidateId, "test-idempotency-key");
        var walletPolicy = new DropWalletPolicy();
        var handler = new DispatchDropCommandHandler(context, walletPolicy);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.ApplicationId);
        
        var updatedWallet = await context.DropWalletBalances.FirstOrDefaultAsync(w => w.CandidateProfileId == candidateId);
        Assert.Equal(4, updatedWallet.DailyRemaining);
    }

    [Fact]
    public async Task Handle_WithInsufficientBalance_ShouldThrowDomainRuleException()
    {
        // Arrange
        var candidateId = Guid.NewGuid();
        var jobId = Guid.NewGuid();
        var rowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 };

        using var context = new ApplicationDbContext(_options);
        var wallet = DropWalletBalance.CreateForTest(
            candidateProfileId: candidateId,
            dailyLimit: 5,
            dailyRemaining: 0,  // No credits
            bonusBalance: 0,
            specialBalance: 0,
            rowVersion: rowVersion);
        
        context.DropWalletBalances.Add(wallet);
        await context.SaveChangesAsync();

        var command = new DispatchDropCommand(jobId, candidateId, "test-idempotency-key");
        var walletPolicy = new DropWalletPolicy();
        var handler = new DispatchDropCommandHandler(context, walletPolicy);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithBonusBalance_ShouldDebitFromBonusWhenDailyEmpty()
    {
        // Arrange
        var candidateId = Guid.NewGuid();
        var jobId = Guid.NewGuid();
        var rowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 };

        using var context = new ApplicationDbContext(_options);
        var wallet = DropWalletBalance.CreateForTest(
            candidateProfileId: candidateId,
            dailyLimit: 5,
            dailyRemaining: 0,
            bonusBalance: 3,
            rowVersion: rowVersion);
        
        context.DropWalletBalances.Add(wallet);
        await context.SaveChangesAsync();

        var command = new DispatchDropCommand(jobId, candidateId, "test-idempotency-key");
        var walletPolicy = new DropWalletPolicy();
        var handler = new DispatchDropCommandHandler(context, walletPolicy);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var updatedWallet = await context.DropWalletBalances.FirstOrDefaultAsync(w => w.CandidateProfileId == candidateId);
        Assert.Equal(0, updatedWallet.DailyRemaining);
        Assert.Equal(2, updatedWallet.BonusBalance);
    }
}

