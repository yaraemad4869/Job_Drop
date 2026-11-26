using MediatR;

namespace JobDrop.Application.Features.Reviews.Commands;

public sealed record SubmitReviewCommand(
    Guid ApplicationId,
    string TargetType,
    Guid TargetId,
    int Rating,
    string? Comment
) : IRequest<Guid>;