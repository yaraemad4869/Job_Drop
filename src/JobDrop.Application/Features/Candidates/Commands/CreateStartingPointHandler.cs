using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Candidates.DTOs;
using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Exceptions;
using JobDrop.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed class CreateStartingPointHandler : IRequestHandler<CreateStartingPointCommand, StartingPointDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUser _currentUser;

    public CreateStartingPointHandler(IApplicationDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<StartingPointDto> Handle(CreateStartingPointCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? throw new ForbiddenAccessException();
        
        var profile = await _db.CandidateProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Candidates.CandidateProfile), userId);

        // Enforce max starting points limit (e.g., 5)
        var count = await _db.StartingPoints.CountAsync(p => p.CandidateProfileId == profile.Id, cancellationToken);
        if (count >= 5) throw new CapacityExceededException("Maximum 5 starting points allowed.");

        var startingPoint = new StartingPoint
        {
            CandidateProfileId = profile.Id,
            Label = request.Label,
            AnchorType = (AnchorType)request.AnchorType,
            Location = new GeoPoint(request.Latitude, request.Longitude),
            IsActiveOrigin = request.IsActiveOrigin
        };

        _db.StartingPoints.Add(startingPoint);
        await _db.SaveChangesAsync(cancellationToken);

        return new StartingPointDto(
            startingPoint.Id, startingPoint.Label, startingPoint.AnchorType,
            startingPoint.Location.Latitude, startingPoint.Location.Longitude, startingPoint.IsActiveOrigin);
    }
}