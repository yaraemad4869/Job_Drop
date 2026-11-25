using Xunit;
using FluentAssertions;
using JobDrop.Application.Features.Auth.Commands;

namespace JobDrop.Application.UnitTests.Features.Auth.Commands;

public class RegisterCandidateCommandValidatorTests
{
    private readonly RegisterCandidateCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldPass()
    {
        var command = new RegisterCandidateCommand("test@example.com", "Password123!", "testuser", "Test User");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("test@")]
    public void InvalidEmail_ShouldFail(string email)
    {
        var command = new RegisterCandidateCommand(email, "Password123!", "testuser", null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("short")]
    [InlineData("nouppercase123!")]
    [InlineData("NOLOWERCASE123!")]
    [InlineData("NoDigits!!")]
    [InlineData("NoSpecial123")]
    public void WeakPassword_ShouldFail(string password)
    {
        var command = new RegisterCandidateCommand("test@example.com", password, "testuser", null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("UPPERCASE")]
    [InlineData("has space")]
    [InlineData("has-dash")]
    public void InvalidHandle_ShouldFail(string handle)
    {
        var command = new RegisterCandidateCommand("test@example.com", "Password123!", handle, null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void HandleTooLong_ShouldFail()
    {
        var command = new RegisterCandidateCommand("test@example.com", "Password123!", "a".PadRight(31, 'a'), null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ValidHandle_ShouldPass()
    {
        var command = new RegisterCandidateCommand("test@example.com", "Password123!", "valid_handle_123", null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}