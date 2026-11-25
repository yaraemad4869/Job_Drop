using MediatR;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed record CreateStartingPointCommand(
    Guid CandidateProfileId,
    string Label,
    string AnchorType,
    double Latitude,
    double Longitude,
    bool IsActiveOrigin) : IRequest<Guid>;
