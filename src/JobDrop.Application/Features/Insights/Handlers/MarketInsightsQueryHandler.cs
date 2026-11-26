using JobDrop.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Insights.Handlers;

public sealed class MarketInsightsQueryHandler
    : IRequestHandler<Queries.MarketInsightsQuery, Queries.MarketInsightsResult>
{
    private readonly IApplicationDbContext _db;

    public MarketInsightsQueryHandler(IApplicationDbContext db) => _db = db;

    public async Task<Queries.MarketInsightsResult> Handle(
        Queries.MarketInsightsQuery request,
        CancellationToken cancellationToken)
    {
        // Fixed: Job.Status is string, compare with "Published"
        var jobs = await _db.Jobs
            .Where(j => j.Status == "Published")
            .AsNoTracking()
            .Take(100)
            .ToListAsync(cancellationToken);

        return new Queries.MarketInsightsResult(
            HighMatchDensityCount: jobs.Count,
            BestNearbyMatch: null,
            CompensationBaseline: null,
            RankedNearbyRoles: Array.Empty<object>(),
            NearbyJobsBySector: Array.Empty<object>(),
            TopSkillGaps: Array.Empty<object>());
    }
}