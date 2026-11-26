using System;
using FluentValidation.TestHelper;
using JobDrop.Application.Features.Candidates.Commands;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

public sealed class UpdateCandidateProfileCommandValidatorTests
{
    private readonly UpdateCandidateProfileCommandValidator _validator;

    public UpdateCandidateProfileCommandValidatorTests()
    {
        _validator = new UpdateCandidateProfileCommandValidator();
    }

    [Fact]
    public void Validate_WithEmptyProfileId_ShouldFail()
    {
        // Arrange
        var command = new UpdateCandidateProfileCommand(
            ProfileId: Guid.Empty,
            Headline: "Valid Headline",
            Summary: "Valid Summary");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProfileId);
    }

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        // Arrange
        var command = new UpdateCandidateProfileCommand(
            ProfileId: Guid.NewGuid(),
            Headline: "Senior .NET Developer",
            Summary: "Experienced backend engineer with 8 years in enterprise systems.");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithNullOptionalFields_ShouldPass()
    {
        // Arrange - Headline and Summary are nullable (partial update)
        var command = new UpdateCandidateProfileCommand(
            ProfileId: Guid.NewGuid(),
            Headline: null,
            Summary: null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithHeadlineExceedingMaxLength_ShouldFail()
    {
        // Arrange
        var longHeadline = new string('A', 201);
        var command = new UpdateCandidateProfileCommand(
            ProfileId: Guid.NewGuid(),
            Headline: longHeadline,
            Summary: null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Headline);
    }

    [Fact]
    public void Validate_WithSummaryExceedingMaxLength_ShouldFail()
    {
        // Arrange
        var longSummary = new string('B', 4001);
        var command = new UpdateCandidateProfileCommand(
            ProfileId: Guid.NewGuid(),
            Headline: null,
            Summary: longSummary);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Summary);
    }

    [Fact]
    public void Validate_WithBoundaryLengths_ShouldPass()
    {
        // Arrange - Exactly at the limit
        var command = new UpdateCandidateProfileCommand(
            ProfileId: Guid.NewGuid(),
            Headline: new string('A', 200),   // Exactly 200 chars
            Summary: new string('B', 4000));  // Exactly 4000 chars

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}