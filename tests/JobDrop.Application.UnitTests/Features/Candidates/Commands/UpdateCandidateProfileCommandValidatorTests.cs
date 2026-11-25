using FluentAssertions;
using JobDrop.Application.Features.Candidates.Commands;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

public class UpdateCandidateProfileCommandValidatorTests
{
    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var validator = new UpdateCandidateProfileCommandValidator();
        var command = new UpdateCandidateProfileCommand(
            CandidateProfileId: Guid.NewGuid(),
            FullName: "Test User",
            Headline: "Developer",
            Summary: "Summary text",
            DropThemeCode: "dark");

        var result = validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyCandidateProfileId_ShouldFail()
    {
        var validator = new UpdateCandidateProfileCommandValidator();
        var command = new UpdateCandidateProfileCommand(
            CandidateProfileId: Guid.Empty,
            FullName: "Test User",
            Headline: null,
            Summary: null,
            DropThemeCode: null);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLongFullName_ShouldFail()
    {
        var validator = new UpdateCandidateProfileCommandValidator();
        var command = new UpdateCandidateProfileCommand(
            CandidateProfileId: Guid.NewGuid(),
            FullName: new string('a', 201),
            Headline: null,
            Summary: null,
            DropThemeCode: null);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLongSummary_ShouldFail()
    {
        var validator = new UpdateCandidateProfileCommandValidator();
        var command = new UpdateCandidateProfileCommand(
            CandidateProfileId: Guid.NewGuid(),
            FullName: null,
            Headline: null,
            Summary: new string('a', 4001),
            DropThemeCode: null);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
