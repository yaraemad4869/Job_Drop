using MediatR;

namespace JobDrop.Application.Features.Discovery.Queries;

public sealed record SearchJobsQuery(
    string? MarketCode,
    string[]? TargetRoles,
    Guid[]? SkillIds,
    string[]? WorkModes,
    int? RadiusKm,
    int? MaxDurationMinutes,
    int? SalaryMin,
    int? SalaryMax,
    bool VerifiedCompaniesOnly,
    string Sort = "MatchDesc"
) : IRequest<SearchJobsResult>;

public sealed record SearchJobsResult(
    string SearchContextId,
    int TotalCount,
    object[] Jobs
);