using MediatR;

namespace JobDrop.Application.Features.RecruiterReview.Commands;

public sealed record InviteToInterviewCommand(Guid DropId) : IRequest<bool>;