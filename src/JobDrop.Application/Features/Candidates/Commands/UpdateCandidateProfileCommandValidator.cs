using FluentValidation;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed class UpdateCandidateProfileCommandValidator : AbstractValidator<UpdateCandidateProfileCommand>
{
    public UpdateCandidateProfileCommandValidator()
    {
        RuleFor(x => x.Payload.Headline)
            .MaximumLength(200).When(x => x.Payload.Headline != null);
        RuleFor(x => x.Payload.Summary)
            .MaximumLength(4000).When(x => x.Payload.Summary != null);
        RuleFor(x => x.Payload.DropThemeCode)
            .MaximumLength(50).When(x => x.Payload.DropThemeCode != null);
    }
}