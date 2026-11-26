using MediatR;

namespace JobDrop.Application.Features.RecruiterReview.Commands;

public sealed record ShortlistDropCommand(Guid DropId) : IRequest<bool>;