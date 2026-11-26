using MediatR;

namespace JobDrop.Application.Features.Drops.Commands;

public sealed record ShortlistDropCommand(Guid DropId) : IRequest<bool>;