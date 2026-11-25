using FluentValidation;

namespace JobDrop.Application.Features.Drops.Commands;

public sealed class DispatchDropCommandValidator : AbstractValidator<DispatchDropCommand>
{
    public DispatchDropCommandValidator()
    {
        RuleFor(x => x.JobId).NotEmpty().WithMessage("JobId is required.");
        RuleFor(x => x.CandidateProfileId).NotEmpty().WithMessage("CandidateProfileId is required.");
        
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty().WithMessage("IdempotencyKey is required.")
            .MaximumLength(100).WithMessage("IdempotencyKey must not exceed 100 characters.")
            .Matches(@"^[a-zA-Z0-9_\-]+$").WithMessage("IdempotencyKey must contain only alphanumeric characters, hyphens, and underscores.");
    }
}
