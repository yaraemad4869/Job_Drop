using FluentValidation;
using JobDrop.Application.Features.Discovery.DTOs;

namespace JobDrop.Application.Features.Discovery.Queries;

public sealed class SpatialSearchCommandValidator : AbstractValidator<SpatialSearchCommand>
{
    public SpatialSearchCommandValidator()
    {
        RuleFor(x => x.TargetRoles)
            .Must(r => r == null || r.Count <= 5)
            .WithMessage("Maximum 5 target roles allowed.");
        RuleFor(x => x.CommuteMode)
            .InclusiveBetween(0, 3).When(x => x.CommuteMode.HasValue);
        RuleFor(x => x.MaxDurationMinutes)
            .InclusiveBetween(1, 180).When(x => x.MaxDurationMinutes.HasValue);
        RuleFor(x => x.RadiusKm)
            .InclusiveBetween(1, 100).When(x => x.RadiusKm.HasValue);
        RuleFor(x => x.SalaryMin)
            .GreaterThanOrEqualTo(0).When(x => x.SalaryMin.HasValue);
        RuleFor(x => x)
            .Must(x => !x.SalaryMin.HasValue || !x.SalaryMax.HasValue || x.SalaryMax >= x.SalaryMin)
            .WithMessage("SalaryMax must be >= SalaryMin.");
    }
}