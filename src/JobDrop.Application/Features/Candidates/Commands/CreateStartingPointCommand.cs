using JobDrop.Application.Features.Candidates.DTOs;
using MediatR;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed record CreateStartingPointCommand(string Label, int AnchorType, double Latitude, double Longitude, bool IsActiveOrigin) : IRequest<StartingPointDto>;