using JobDrop.Application.UnitTests;
using FluentAssertions;
using JobDrop.Application.Features.Candidates.Commands;
using JobDrop.Domain.Entities.Candidates;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

public class UpdateCandidateProfileHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly UpdateCandidateProfileHandler _handler;

    public UpdateCandidateProfileHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new UpdateCandidateProfileHandler(_context);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldUpdateProfile()
    {
        // Arrange
        var existingProfile = new CandidateProfile(Guid.NewGuid(), "Original Name", "Original Title");
        _context.CandidateProfiles.Add(existingProfile);
        await _context.SaveChangesAsync();

        var command = new UpdateCandidateProfileCommand(
            CandidateProfileId: existingProfile.Id,
            FullName: "Updated Name",
            Headline: "Senior Developer",
            Summary: "Experienced developer",
            DropThemeCode: "dark");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        
        var updatedProfile = await _context.CandidateProfiles
            .FirstOrDefaultAsync(p => p.Id == existingProfile.Id);
        
        updatedProfile.Should().NotBeNull();
        updatedProfile!.FullName.Should().Be("Updated Name");
        updatedProfile.Headline.Should().Be("Senior Developer");
        updatedProfile.Summary.Should().Be("Experienced developer");
        updatedProfile.DropThemeCode.Should().Be("dark");
    }

    [Fact]
    public async Task Handle_WithPartialUpdate_ShouldUpdateOnlyProvidedFields()
    {
        // Arrange
        var existingProfile = new CandidateProfile(Guid.NewGuid(), "Original Name", "Original Title");
        _context.CandidateProfiles.Add(existingProfile);
        await _context.SaveChangesAsync();

        var command = new UpdateCandidateProfileCommand(
            CandidateProfileId: existingProfile.Id,
            FullName: null,
            Headline: "Updated Headline Only",
            Summary: null,
            DropThemeCode: null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        
        var updatedProfile = await _context.CandidateProfiles
            .FirstOrDefaultAsync(p => p.Id == existingProfile.Id);
        
        updatedProfile.Should().NotBeNull();
        updatedProfile!.FullName.Should().Be("Original Name"); // Unchanged
        updatedProfile.Headline.Should().Be("Updated Headline Only"); // Changed
        updatedProfile.Summary.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
