using FluentValidation;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed class CreateCompanyOfficeCommandValidator : AbstractValidator<CreateCompanyOfficeCommand>
{
    public CreateCompanyOfficeCommandValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Nickname).NotEmpty().MaximumLength(150);
        RuleFor(x => x.OfficeType)
            .InclusiveBetween(0, 2).WithMessage("OfficeType must be 0 (HQ), 1 (Satellite), or 2 (Booth).");
        RuleFor(x => x.Latitude).InclusiveBetween(-90.0, 90.0);
        RuleFor(x => x.Longitude).InclusiveBetween(-180.0, 180.0);
        RuleFor(x => x.GeofenceRadiusMeters)
            .InclusiveBetween(50, 50000).WithMessage("GeofenceRadius must be between 50m and 50km.");
        RuleFor(x => x.MakaniCode).MaximumLength(50).When(x => x.MakaniCode != null);
    }
}