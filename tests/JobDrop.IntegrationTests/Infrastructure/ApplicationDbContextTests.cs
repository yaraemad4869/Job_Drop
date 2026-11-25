using FluentAssertions;
using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Infrastructure.Persistence;
using JobDrop.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JobDrop.IntegrationTests.Infrastructure;

public class ApplicationDbContextTests : IClassFixture<JobDropWebAppFactory>
{
    private readonly JobDropWebAppFactory _factory;

    public ApplicationDbContextTests(JobDropWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CanInsertAndRetrieveCandidateProfile()
    {
        using var db = _factory.CreateDbContext();

        var uniqueId = Guid.NewGuid().ToString("N")[..8];
        var user = new AppUser
        {
            Email = $"test_{uniqueId}@example.com",
            UserName = $"test_{uniqueId}",
            Handle = $"handle_{uniqueId}",
            EmailConfirmed = true
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var profile = new CandidateProfile
        {
            UserId = user.Id,
            Headline = "Test Headline",
            Summary = "Test Summary"
            // RowVersion will be null, which is now safely allowed by our factory setup
        };
        
        db.CandidateProfiles.Add(profile);
        await db.SaveChangesAsync();

        var retrieved = await db.CandidateProfiles.FirstOrDefaultAsync(p => p.Id == profile.Id);

        retrieved.Should().NotBeNull();
        retrieved!.Headline.Should().Be("Test Headline");
        retrieved.UserId.Should().Be(user.Id);
    }

    [Fact]
    public async Task CanInsertAndRetrieveDropWalletBalance()
    {
        using var db = _factory.CreateDbContext();

        var uniqueId = Guid.NewGuid().ToString("N")[..8];
        var user = new AppUser
        {
            Email = $"wallet_{uniqueId}@example.com",
            UserName = $"wallet_{uniqueId}",
            Handle = $"handle_w_{uniqueId}",
            EmailConfirmed = true
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var profile = new CandidateProfile 
        { 
            UserId = user.Id
            // RowVersion will be null
        };
        db.CandidateProfiles.Add(profile);
        await db.SaveChangesAsync();

        var wallet = new DropWalletBalance
        {
            CandidateProfileId = profile.Id,
            DailyRemaining = 5,
            DailyLimit = 5,
            DailyResetAtUtc = DateTimeOffset.UtcNow.Date.AddDays(1),
            BonusBalance = 0,
            SpecialBalance = 0
        };
        db.DropWalletBalances.Add(wallet);
        await db.SaveChangesAsync();

        var retrieved = await db.DropWalletBalances.FirstOrDefaultAsync(w => w.CandidateProfileId == profile.Id);

        retrieved.Should().NotBeNull();
        retrieved!.DailyRemaining.Should().Be(5);
    }

    [Fact]
    public async Task UniqueConstraint_Handle_ShouldPreventDuplicates()
    {
        using var db = _factory.CreateDbContext();

        var uniqueId = Guid.NewGuid().ToString("N")[..8];
        var handle = $"unique_{uniqueId}";

        var user1 = new AppUser { Email = $"u1_{uniqueId}@ex.com", UserName = $"u1_{uniqueId}", Handle = handle, EmailConfirmed = true };
        db.Users.Add(user1);
        await db.SaveChangesAsync();

        var user2 = new AppUser { Email = $"u2_{uniqueId}@ex.com", UserName = $"u2_{uniqueId}", Handle = handle, EmailConfirmed = true };
        db.Users.Add(user2);

        var act = async () => await db.SaveChangesAsync();
        await act.Should().ThrowAsync<DbUpdateException>();
    }
}