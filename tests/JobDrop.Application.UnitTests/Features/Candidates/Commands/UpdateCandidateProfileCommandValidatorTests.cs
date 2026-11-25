using Xunit;
using FluentAssertions;
using JobDrop.Application.Features.Candidates.Commands;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

public class UpdateCandidateProfileCommandValidatorTests
{
    private readonly UpdateCandidateProfileCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldPass()
    {
        var command = new UpdateCandidateProfileCommand(
            new UpdateCandidateProfileCommand.Data("Headline", "Summary", true, "classic"));
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void NullPayload_ShouldPass()
    {
        var command = new UpdateCandidateProfileCommand(
            new UpdateCandidateProfileCommand.Data(null, null, null, null));
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void HeadlineTooLong_ShouldFail()
    {
        var command = new UpdateCandidateProfileCommand(
            new UpdateCandidateProfileCommand.Data("a".PadRight(201, 'a'), null, null, null));
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SummaryTooLong_ShouldFail()
    {
        var command = new UpdateCandidateProfileCommand(
            new UpdateCandidateProfileCommand.Data(null, "a".PadRight(4001, 'a'), null, null));
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }
}