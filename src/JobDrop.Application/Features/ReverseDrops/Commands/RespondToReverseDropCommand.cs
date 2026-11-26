using MediatR;

namespace JobDrop.Application.Features.ReverseDrops.Commands;

public sealed record RespondToReverseDropCommand(Guid ReverseDropId, bool Accept) : IRequest<bool>;