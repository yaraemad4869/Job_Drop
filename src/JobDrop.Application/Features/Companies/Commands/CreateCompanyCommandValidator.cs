using FluentValidation;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.LegalName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Website).MaximumLength(500);
    }
}