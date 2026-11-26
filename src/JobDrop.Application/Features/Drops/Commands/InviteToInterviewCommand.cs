using MediatR;

namespace JobDrop.Application.Features.Drops.Commands;

public sealed record InviteToInterviewCommand(Guid DropId) : IRequest<bool>;