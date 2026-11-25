using Xunit;
using FluentAssertions;
using JobDrop.Application.Features.Candidates.Commands;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

public class CreateStartingPointCommandValidatorTests
{
    private readonly CreateStartingPointCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldPass()
    {
        var command = new CreateStartingPointCommand("Home", 0, 30.0444, 31.2357, true);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void EmptyLabel_ShouldFail()
    {
        var command = new CreateStartingPointCommand("", 0, 30.0444, 31.2357, true);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    public void InvalidAnchorType_ShouldFail(int anchorType)
    {
        var command = new CreateStartingPointCommand("Home", anchorType, 30.0444, 31.2357, true);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(-91.0)]
    [InlineData(91.0)]
    public void InvalidLatitude_ShouldFail(double latitude)
    {
        var command = new CreateStartingPointCommand("Home", 0, latitude, 31.2357, true);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(-181.0)]
    [InlineData(181.0)]
    public void InvalidLongitude_ShouldFail(double longitude)
    {
        var command = new CreateStartingPointCommand("Home", 0, 30.0444, longitude, true);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void NullIsland_ShouldFail()
    {
        var command = new CreateStartingPointCommand("Home", 0, 0, 0, true);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }
}