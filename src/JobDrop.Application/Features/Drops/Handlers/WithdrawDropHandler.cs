using MediatR;
using JobDrop.Application.Features.Drops.Commands;

namespace JobDrop.Application.Features.Drops.Handlers;

public class WithdrawDropHandler : IRequestHandler<WithdrawDropCommand, bool>
{
    public Task<bool> Handle(WithdrawDropCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement drop withdrawal with refund logic
        throw new NotImplementedException("WithdrawDropHandler is not yet implemented");
    }
}
