using FluentValidation;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(256);
        RuleFor(x => x.DeviceInfo).MaximumLength(500);
    }
}