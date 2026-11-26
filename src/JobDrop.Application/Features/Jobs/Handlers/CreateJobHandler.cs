using MediatR;
using JobDrop.Application.Features.Jobs.Commands;

namespace JobDrop.Application.Features.Jobs.Handlers;

public class CreateJobHandler : IRequestHandler<CreateJobCommand, Guid>
{
    public Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement job creation with validation, coverage whitelist, etc.
        throw new NotImplementedException("CreateJobHandler is not yet implemented");
    }
}
