using FluentValidation;

namespace JobDrop.Application.Features.Candidates.Commands;

/// <summary>
/// Validates UpdateCandidateProfileCommand inputs.
/// Enforces length limits and non-empty ProfileId.
/// </summary>
public sealed class UpdateCandidateProfileCommandValidator : AbstractValidator<UpdateCandidateProfileCommand>
{
    public UpdateCandidateProfileCommandValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty()
            .WithMessage("ProfileId is required.");

        RuleFor(x => x.Headline)
            .MaximumLength(200)
            .When(x => x.Headline != null)
            .WithMessage("Headline cannot exceed 200 characters.");

        RuleFor(x => x.Summary)
            .MaximumLength(4000)
            .When(x => x.Summary != null)
            .WithMessage("Summary cannot exceed 4000 characters.");
    }
}