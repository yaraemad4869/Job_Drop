using FluentAssertions;
using JobDrop.Application.Features.Drops.Commands;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Entities.Jobs;
using JobDrop.Domain.Exceptions;
using JobDrop.Domain.Enums;
using JobDrop.Application.UnitTests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Drops.Commands;

public class DispatchDropHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly DispatchDropHandler _handler;

    public DispatchDropHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new DispatchDropHandler(_context);
    }

    [Fact]
    public async Task Handle_WithExistingIdempotencyKey_ShouldThrowDomainRuleException()
    {
        var command = new DispatchDropCommand(Guid.NewGuid(), Guid.NewGuid(), "existing_key");
        var existingLedger = new DropLedgerEntry(Guid.NewGuid(), WalletBucket.Daily, -1, "TEST", "ref", "existing_key");
        _context.DropLedgerEntries.Add(existingLedger);
        await _context.SaveChangesAsync();

        var act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<DomainRuleException>().WithMessage("*idempotency*");
    }

    [Fact]
    public async Task Handle_WithInsufficientBalance_ShouldThrowDomainRuleException()
    {
        // 1. Create and save Job FIRST to get its generated Id
        var job = new Job(Guid.NewGuid(), "Test Job", "Description", "Remote", "Full-time");
        job.SetIntakeLimit(10);
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        // 2. Use the SAME job.Id in the command
        var command = new DispatchDropCommand(job.Id, Guid.NewGuid(), "new_key_123");
        var emptyWallet = new DropWalletBalance(command.CandidateProfileId, 0);
        _context.DropWalletBalances.Add(emptyWallet);
        await _context.SaveChangesAsync();

        var act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<DomainRuleException>().WithMessage("*Insufficient Drop credits*");
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldDebitWalletAndCreateSla()
    {
        // 1. Create and save Job FIRST to get its generated Id
        var job = new Job(Guid.NewGuid(), "Test Job", "Description", "Remote", "Full-time");
        job.SetIntakeLimit(10);
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        // 2. Use the SAME job.Id in the command
        var command = new DispatchDropCommand(job.Id, Guid.NewGuid(), "success_key_999");
        var wallet = new DropWalletBalance(command.CandidateProfileId, 5);
        _context.DropWalletBalances.Add(wallet);
        await _context.SaveChangesAsync();

        // 3. Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // 4. Assert
        result.Should().NotBeNull();
        result.ConsumedFrom.Should().Be(WalletBucket.Daily);
        
        var updatedWallet = await _context.DropWalletBalances.FirstOrDefaultAsync(w => w.CandidateProfileId == command.CandidateProfileId);
        updatedWallet!.DailyRemaining.Should().Be(4);

        var ledgerEntry = await _context.DropLedgerEntries.FirstOrDefaultAsync(l => l.IdempotencyKey == "success_key_999");
        ledgerEntry.Should().NotBeNull();
        ledgerEntry!.Delta.Should().Be(-1);

        var sla = await _context.ApplicationSlas.FirstOrDefaultAsync(s => s.ApplicationId == result.ApplicationId);
        sla.Should().NotBeNull();
        sla!.Status.Should().Be(SlaStatus.Pending);
    }

    public void Dispose() => _context.Dispose();
}
