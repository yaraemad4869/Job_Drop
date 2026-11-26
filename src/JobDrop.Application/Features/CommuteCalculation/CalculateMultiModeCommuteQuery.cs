using MediatR;
using JobDrop.Domain.Enums;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.Application.Features.CommuteCalculation;

public record CalculateMultiModeCommuteQuery(CalculateCommuteRequest Request) : IRequest<MultiModeCommuteResponse>;

public record MultiModeCommuteResponse(List<CommuteModeResult> Results);

public record CommuteModeResult(CommuteMode Mode, int DurationMinutes, double DistanceKm, bool SatisfiesThreshold);
