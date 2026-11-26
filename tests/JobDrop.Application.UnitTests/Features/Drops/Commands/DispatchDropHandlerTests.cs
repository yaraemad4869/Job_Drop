using System;
using System.Threading;
using System.Threading.Tasks;
using JobDrop.Application.Features.Drops.Commands;
using JobDrop.Application.Features.Drops.Handlers;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Entities.Jobs;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Exceptions;
using JobDrop.Domain.Services;
using JobDrop.Infrastructure.Persistence;
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
    public async Task Handle_WithValidRequest_ShouldDebitWalletAndCreateApplication()
    {
        // Arrange
        var candidateId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var rowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 };

        using var context = new ApplicationDbContext(_options);
        
        // 1. Seed Job (job.Id is generated internally by the entity/EF Core)
        var job = new Job(companyId, "Test Job", "Test Description", "Remote", "Full-time");
        context.Jobs.Add(job);

        // 2. Seed JobDailyIntake using the ACTUAL job.Id
        var intake = JobDailyIntake.CreateForTest(
            jobId: job.Id,
            marketDate: DateOnly.FromDateTime(DateTime.UtcNow),
            deliveredCount: 0);
        context.JobDailyIntakes.Add(intake);

        // 3. Seed Wallet
        var wallet = DropWalletBalance.CreateForTest(
            candidateProfileId: candidateId,
            dailyLimit: 5,
            dailyRemaining: 5,
            rowVersion: rowVersion);
        context.DropWalletBalances.Add(wallet);
        
        await context.SaveChangesAsync();

        // Use job.Id for the command, NOT a random Guid!
        var command = new DispatchDropCommand(job.Id, candidateId, "test-idempotency-key");
        var walletPolicy = new DropWalletPolicy();
        var handler = new DispatchDropCommandHandler(context, walletPolicy);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.ApplicationId);
        Assert.Equal(WalletBucket.Daily, result.ConsumedFrom);
        
        var updatedWallet = await context.DropWalletBalances.FirstOrDefaultAsync(w => w.CandidateProfileId == candidateId);
        Assert.Equal(4, updatedWallet.DailyRemaining);
        
        var updatedIntake = await context.JobDailyIntakes.FirstOrDefaultAsync(i => i.JobId == job.Id);
        Assert.Equal(1, updatedIntake.DeliveredCount);
    }

    [Fact]
    public async Task Handle_WithInsufficientBalance_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var candidateId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var rowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 };

        using var context = new ApplicationDbContext(_options);
        
        var job = new Job(companyId, "Test Job", "Test Description", "Remote", "Full-time");
        context.Jobs.Add(job);

        var intake = JobDailyIntake.CreateForTest(
            jobId: job.Id,
            marketDate: DateOnly.FromDateTime(DateTime.UtcNow),
            deliveredCount: 0);
        context.JobDailyIntakes.Add(intake);

        var wallet = DropWalletBalance.CreateForTest(
            candidateProfileId: candidateId,
            dailyLimit: 5,
            dailyRemaining: 0,
            bonusBalance: 0,
            specialBalance: 0,
            rowVersion: rowVersion);
        context.DropWalletBalances.Add(wallet);
        
        await context.SaveChangesAsync();

        var command = new DispatchDropCommand(job.Id, candidateId, "test-idempotency-key-2");
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
        var companyId = Guid.NewGuid();
        var rowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 };

        using var context = new ApplicationDbContext(_options);
        
        var job = new Job(companyId, "Test Job", "Test Description", "Remote", "Full-time");
        context.Jobs.Add(job);

        var intake = JobDailyIntake.CreateForTest(
            jobId: job.Id,
            marketDate: DateOnly.FromDateTime(DateTime.UtcNow),
            deliveredCount: 0);
        context.JobDailyIntakes.Add(intake);

        var wallet = DropWalletBalance.CreateForTest(
            candidateProfileId: candidateId,
            dailyLimit: 5,
            dailyRemaining: 0,
            bonusBalance: 3,
            specialBalance: 0,
            rowVersion: rowVersion);
        context.DropWalletBalances.Add(wallet);
        
        await context.SaveChangesAsync();

        var command = new DispatchDropCommand(job.Id, candidateId, "test-idempotency-key-3");
        var walletPolicy = new DropWalletPolicy();
        var handler = new DispatchDropCommandHandler(context, walletPolicy);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(WalletBucket.Bonus, result.ConsumedFrom);
        
        var updatedWallet = await context.DropWalletBalances.FirstOrDefaultAsync(w => w.CandidateProfileId == candidateId);
        Assert.Equal(0, updatedWallet.DailyRemaining);
        Assert.Equal(2, updatedWallet.BonusBalance);
    }
}