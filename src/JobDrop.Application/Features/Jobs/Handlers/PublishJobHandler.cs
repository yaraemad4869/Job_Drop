using MediatR;
using JobDrop.Application.Features.Jobs.Commands;

namespace JobDrop.Application.Features.Jobs.Handlers;

public class PublishJobHandler : IRequestHandler<PublishJobCommand, bool>
{
    public Task<bool> Handle(PublishJobCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement job publishing with state transitions
        throw new NotImplementedException("PublishJobHandler is not yet implemented");
    }
}
