using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Candidates;
using MediatR;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed class CreateStartingPointHandler : IRequestHandler<CreateStartingPointCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateStartingPointHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateStartingPointCommand request, CancellationToken cancellationToken)
    {
        var startingPoint = new StartingPoint(
            request.CandidateProfileId,
            request.Label,
            request.AnchorType,
            request.Latitude,
            request.Longitude);

        if (!request.IsActiveOrigin)
        {
            startingPoint.Deactivate();
        }

        _context.StartingPoints.Add(startingPoint);
        await _context.SaveChangesAsync(cancellationToken);

        return startingPoint.Id;
    }
}
