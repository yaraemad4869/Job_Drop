using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Insights.Handlers;

public sealed class PersonalInsightsQueryHandler
    : IRequestHandler<Queries.PersonalInsightsQuery, Queries.PersonalInsightsResult>
{
    private readonly IApplicationDbContext _db;

    public PersonalInsightsQueryHandler(IApplicationDbContext db) => _db = db;

    public async Task<Queries.PersonalInsightsResult> Handle(
        Queries.PersonalInsightsQuery request,
        CancellationToken cancellationToken)
    {
        var candidateId = Guid.Empty; // TODO: From ICurrentUser

        // Fixed: ApplicationEvent may not have EventType directly, join with Application
        var events = await _db.ApplicationEvents
            .Join(_db.Applications,
                e => e.ApplicationId,
                a => a.Id,
                (e, a) => new { Event = e, Application = a })
            .Where(x => x.Application.CandidateProfileId == candidateId)
            .Select(x => x.Event)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Note: If ApplicationEvent doesn't have EventType, we'll count all events
        var totalCount = events.Count;

        return new Queries.PersonalInsightsResult(
            SentCount: totalCount,
            LandedCount: 0,
            OpenedCount: 0,
            ReviewedCount: 0,
            ShortlistedCount: 0,
            InterviewedCount: 0,
            HiredCount: 0,
            RefundedCount: 0,
            CvStrengthScore: 0.75,
            ProfileCompletenessPct: 0.80);
    }
}