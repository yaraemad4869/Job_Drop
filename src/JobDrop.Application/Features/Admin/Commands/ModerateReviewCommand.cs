using MediatR;

namespace JobDrop.Application.Features.Admin.Commands;

public sealed record ModerateReviewCommand(Guid ReviewId, string Decision) : IRequest<bool>;