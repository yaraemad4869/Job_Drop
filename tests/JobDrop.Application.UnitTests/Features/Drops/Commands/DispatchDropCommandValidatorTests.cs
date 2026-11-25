using FluentAssertions;
using JobDrop.Application.Features.Drops.Commands;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Drops.Commands;

public class DispatchDropCommandValidatorTests
{
    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var validator = new DispatchDropCommandValidator();
        var command = new DispatchDropCommand(
            JobId: Guid.NewGuid(),
            CandidateProfileId: Guid.NewGuid(),
            IdempotencyKey: "dispatch_12345_abc");

        var result = validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithInvalidIdempotencyKey_ShouldFail()
    {
        var validator = new DispatchDropCommandValidator();
        var command = new DispatchDropCommand(
            JobId: Guid.NewGuid(),
            CandidateProfileId: Guid.NewGuid(),
            IdempotencyKey: "invalid key with spaces!"); // Fails regex

        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "IdempotencyKey");
    }

    [Fact]
    public void Validate_WithMissingIds_ShouldFail()
    {
        var validator = new DispatchDropCommandValidator();
        var command = new DispatchDropCommand(
            JobId: Guid.Empty,
            CandidateProfileId: Guid.Empty,
            IdempotencyKey: "valid_key_123");

        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }
}
