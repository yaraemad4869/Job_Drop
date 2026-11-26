using MediatR;

namespace JobDrop.Application.Features.Insights.Queries;

public sealed record MarketInsightsQuery(
    string ScopeType,
    Guid ScopeId,
    string? SearchContextId) : IRequest<MarketInsightsResult>;

public sealed record MarketInsightsResult(
    int HighMatchDensityCount,
    object? BestNearbyMatch,
    object? CompensationBaseline,
    object[] RankedNearbyRoles,
    object[] NearbyJobsBySector,
    object[] TopSkillGaps);