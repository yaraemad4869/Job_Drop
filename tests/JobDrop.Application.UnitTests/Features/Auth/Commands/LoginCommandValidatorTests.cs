using Xunit;
using FluentAssertions;
using JobDrop.Application.Features.Auth.Commands;

namespace JobDrop.Application.UnitTests.Features.Auth.Commands;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldPass()
    {
        var command = new LoginCommand("test@example.com", "password", "Chrome");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void EmptyEmail_ShouldFail()
    {
        var command = new LoginCommand("", "password", null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void EmptyPassword_ShouldFail()
    {
        var command = new LoginCommand("test@example.com", "", null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void NullDeviceInfo_ShouldPass()
    {
        var command = new LoginCommand("test@example.com", "password", null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}