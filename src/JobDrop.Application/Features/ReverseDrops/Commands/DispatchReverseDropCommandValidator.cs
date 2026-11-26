using FluentValidation;

namespace JobDrop.Application.Features.ReverseDrops.Commands;

public sealed class DispatchReverseDropCommandValidator : AbstractValidator<DispatchReverseDropCommand>
{
    public DispatchReverseDropCommandValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.JobId).NotEmpty();
        RuleFor(x => x.CandidateProfileId).NotEmpty();
        RuleFor(x => x.IdempotencyKey).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DispatchNote).MaximumLength(2000);
    }
}