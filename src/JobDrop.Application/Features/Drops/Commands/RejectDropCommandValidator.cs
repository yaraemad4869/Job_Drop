using FluentValidation;

namespace JobDrop.Application.Features.Drops.Commands;

public sealed class RejectDropCommandValidator : AbstractValidator<RejectDropCommand>
{
    public RejectDropCommandValidator()
    {
        RuleFor(x => x.DropId).NotEmpty();
        RuleFor(x => x.RejectionReasonCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Feedback).MaximumLength(2000);
    }
}