using FluentValidation;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed class RegisterCandidateCommandValidator : AbstractValidator<RegisterCandidateCommand>
{
    public RegisterCandidateCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password)
            .NotEmpty().MinimumLength(10)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one non-alphanumeric character.");
        RuleFor(x => x.Handle)
            .NotEmpty().MaximumLength(30)
            .Matches("^[a-z0-9_]+$").WithMessage("Handle must be lowercase alphanumeric or underscore.");
        RuleFor(x => x.FullName).MaximumLength(150);
    }
}