using FluentValidation;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed class CreateStartingPointCommandValidator : AbstractValidator<CreateStartingPointCommand>
{
    public CreateStartingPointCommandValidator()
    {
        RuleFor(x => x.CandidateProfileId).NotEmpty();
        RuleFor(x => x.Label).NotEmpty().MaximumLength(100);
        RuleFor(x => x.AnchorType).NotEmpty().Must(type => 
            type == "Primary" || type == "Secondary" || type == "Coworking")
            .WithMessage("AnchorType must be Primary, Secondary, or Coworking");
        RuleFor(x => x.Latitude).InclusiveBetween(-90.0, 90.0);
        RuleFor(x => x.Longitude).InclusiveBetween(-180.0, 180.0);
    }
}
