using FluentValidation;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed class CreateCompanyOfficeCommandValidator : AbstractValidator<CreateCompanyOfficeCommand>
{
    public CreateCompanyOfficeCommandValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Nickname).NotEmpty().MaximumLength(150);
        RuleFor(x => x.OfficeType).NotEmpty().Must(type => 
            type == "HQ" || type == "Satellite" || type == "Booth")
            .WithMessage("OfficeType must be HQ, Satellite, or Booth");
        RuleFor(x => x.Latitude).InclusiveBetween(-90.0, 90.0);
        RuleFor(x => x.Longitude).InclusiveBetween(-180.0, 180.0);
        RuleFor(x => x.GeofenceRadiusMeters).InclusiveBetween(100, 5000);
        RuleFor(x => x.TransitAnchorText).MaximumLength(250).When(x => !string.IsNullOrEmpty(x.TransitAnchorText));
    }
}
