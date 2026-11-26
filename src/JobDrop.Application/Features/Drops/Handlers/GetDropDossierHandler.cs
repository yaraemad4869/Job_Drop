using MediatR;
using JobDrop.Application.Features.Drops.Queries;

namespace JobDrop.Application.Features.Drops.Handlers;

public class GetDropDossierHandler : IRequestHandler<GetDropDossierQuery, object>
{
    public Task<object> Handle(GetDropDossierQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement dossier retrieval with IDOR checks and disclosure projection
        throw new NotImplementedException("GetDropDossierHandler is not yet implemented");
    }
}
