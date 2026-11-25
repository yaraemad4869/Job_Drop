using FluentAssertions;
using JobDrop.Application.Features.Auth.Commands;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Auth.Commands;

public class RegisterCandidateCommandValidatorTests
{
    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var validator = new RegisterCandidateCommandValidator();
        var command = new RegisterCandidateCommand(
            Email: "test@example.com",
            Password: "Test@12345",
            Handle: "testuser",
            FullName: "Test User",
            Headline: "Software Engineer");

        var result = validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldFail()
    {
        var validator = new RegisterCandidateCommandValidator();
        var command = new RegisterCandidateCommand(
            Email: "invalid-email",
            Password: "Test@12345",
            Handle: "testuser",
            FullName: "Test User",
            Headline: null);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithWeakPassword_ShouldFail()
    {
        var validator = new RegisterCandidateCommandValidator();
        var command = new RegisterCandidateCommand(
            Email: "test@example.com",
            Password: "weak",
            Handle: "testuser",
            FullName: "Test User",
            Headline: null);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithEmptyHandle_ShouldFail()
    {
        var validator = new RegisterCandidateCommandValidator();
        var command = new RegisterCandidateCommand(
            Email: "test@example.com",
            Password: "Test@12345",
            Handle: "",
            FullName: "Test User",
            Headline: null);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithEmptyFullName_ShouldFail()
    {
        var validator = new RegisterCandidateCommandValidator();
        var command = new RegisterCandidateCommand(
            Email: "test@example.com",
            Password: "Test@12345",
            Handle: "testuser",
            FullName: "",
            Headline: null);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithNullHeadline_ShouldPass()
    {
        var validator = new RegisterCandidateCommandValidator();
        var command = new RegisterCandidateCommand(
            Email: "test@example.com",
            Password: "Test@12345",
            Handle: "testuser",
            FullName: "Test User",
            Headline: null);

        var result = validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
