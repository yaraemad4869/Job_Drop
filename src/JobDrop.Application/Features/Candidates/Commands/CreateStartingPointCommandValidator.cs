using FluentValidation;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed class CreateStartingPointCommandValidator : AbstractValidator<CreateStartingPointCommand>
{
    public CreateStartingPointCommandValidator()
    {
        RuleFor(x => x.Label)
            .NotEmpty().MaximumLength(150);
        RuleFor(x => x.AnchorType)
            .InclusiveBetween(0, 2).WithMessage("AnchorType must be 0 (Primary), 1 (Secondary), or 2 (Coworking).");
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90.0, 90.0).WithMessage("Latitude must be between -90 and 90.");
        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180.0, 180.0).WithMessage("Longitude must be between -180 and 180.");
        RuleFor(x => x)
            .Must(x => !(x.Latitude == 0 && x.Longitude == 0))
            .WithMessage("Null Island (0,0) coordinates are not allowed.");
    }
}