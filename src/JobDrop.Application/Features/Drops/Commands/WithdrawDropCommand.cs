using MediatR;

namespace JobDrop.Application.Features.Drops.Commands;

public sealed record WithdrawDropCommand(Guid ApplicationId) : IRequest<bool>;