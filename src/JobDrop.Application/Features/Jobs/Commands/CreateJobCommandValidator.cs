using FluentValidation;

namespace JobDrop.Application.Features.Jobs.Commands;

public sealed class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.OfficeId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(10000);
        RuleFor(x => x.DailyIntakeCap).InclusiveBetween(1, 1000);
        RuleFor(x => x.SalaryMin).GreaterThanOrEqualTo(0);
        When(x => x.SalaryMax.HasValue && x.SalaryMin.HasValue, () =>
        {
            RuleFor(x => x.SalaryMax).GreaterThanOrEqualTo(x => x.SalaryMin);
        });
    }
}