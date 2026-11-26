using JobDrop.Infrastructure.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using JobDrop.Application.Features.Candidates.Commands;
using JobDrop.Application.Features.Candidates.Handlers;
using JobDrop.Domain.Entities.Candidates;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

public sealed class UpdateCandidateProfileHandlerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public UpdateCandidateProfileHandlerTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldUpdateProfile()
    {
        // Arrange - Use factory method to respect encapsulation
        var userId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var rowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 };

        using var context = new ApplicationDbContext(_options);
        var profile = CandidateProfile.CreateForTest(
            userId: userId,
            fullName: "Test User",
            id: profileId,
            headline: "Old Headline",
            summary: "Old Summary",
            rowVersion: rowVersion);
        
        context.CandidateProfiles.Add(profile);
        await context.SaveChangesAsync();

        var command = new UpdateCandidateProfileCommand(profileId, "New Headline", "New Summary");
        var handler = new UpdateCandidateProfileHandler(context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        var updatedProfile = await context.CandidateProfiles.FindAsync(profileId);
        Assert.NotNull(updatedProfile);
        Assert.Equal("New Headline", updatedProfile.Headline);
        Assert.Equal("New Summary", updatedProfile.Summary);
    }

    [Fact]
    public async Task Handle_WithPartialUpdate_ShouldUpdateOnlyProvidedFields()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var rowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 };

        using var context = new ApplicationDbContext(_options);
        var profile = CandidateProfile.CreateForTest(
            userId: userId,
            fullName: "Test User",
            id: profileId,
            headline: "Old Headline",
            summary: "Old Summary",
            rowVersion: rowVersion);
        
        context.CandidateProfiles.Add(profile);
        await context.SaveChangesAsync();

        // Update only Headline
        var command = new UpdateCandidateProfileCommand(profileId, "New Headline", null);
        var handler = new UpdateCandidateProfileHandler(context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        var updatedProfile = await context.CandidateProfiles.FindAsync(profileId);
        Assert.NotNull(updatedProfile);
        Assert.Equal("New Headline", updatedProfile.Headline);
        Assert.Equal("Old Summary", updatedProfile.Summary); // Should remain unchanged
    }

    [Fact]
    public async Task Handle_WithNonExistentProfile_ShouldReturnFalse()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        var command = new UpdateCandidateProfileCommand(Guid.NewGuid(), "New Headline", null);
        var handler = new UpdateCandidateProfileHandler(context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}
