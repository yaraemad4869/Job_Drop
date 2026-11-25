using FluentValidation;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed class UpdateCandidateProfileCommandValidator : AbstractValidator<UpdateCandidateProfileCommand>
{
    public UpdateCandidateProfileCommandValidator()
    {
        RuleFor(x => x.CandidateProfileId).NotEmpty();
        
        RuleFor(x => x.FullName)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.FullName));
        
        RuleFor(x => x.Headline)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Headline));
        
        RuleFor(x => x.Summary)
            .MaximumLength(4000)
            .When(x => !string.IsNullOrEmpty(x.Summary));
        
        RuleFor(x => x.DropThemeCode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.DropThemeCode));
    }
}
