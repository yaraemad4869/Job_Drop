using FluentValidation;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed class RegisterCandidateCommandValidator : AbstractValidator<RegisterCandidateCommand>
{
    public RegisterCandidateCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Handle).NotEmpty().MaximumLength(50);
        
        // FIX: Added NotEmpty() to catch empty strings
        RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required.").MaximumLength(200);
        
        RuleFor(x => x.Headline).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Headline));
    }
}
