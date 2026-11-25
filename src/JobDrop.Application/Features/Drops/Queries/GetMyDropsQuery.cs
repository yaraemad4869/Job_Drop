using MediatR;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Drops.Queries;

public sealed record GetMyDropsQuery(Guid CandidateProfileId) : IRequest<List<MyDropDto>>;

public sealed record MyDropDto(
    Guid ApplicationId,
    string JobTitle,
    string CompanyName,
    SlaStatus SlaStatus,
    DateTime AppliedAtUtc,
    DateTime DueAtUtc,
    bool IsExpired);

public sealed class GetMyDropsQueryHandler : IRequestHandler<GetMyDropsQuery, List<MyDropDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMyDropsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MyDropDto>> Handle(GetMyDropsQuery request, CancellationToken cancellationToken)
    {
        var applications = await _context.Applications
            .Where(a => a.CandidateProfileId == request.CandidateProfileId)
            .ToListAsync(cancellationToken);

        if (applications.Count == 0) return new List<MyDropDto>();

        var appById = applications.ToDictionary(a => a.Id);
        var appIds = appById.Keys.ToList();

        var slas = await _context.ApplicationSlas
            .Where(s => appIds.Contains(s.ApplicationId))
            .OrderByDescending(s => s.StartedAtUtc)
            .ToListAsync(cancellationToken);

        var jobIds = applications.Select(a => a.JobId).Distinct().ToList();

        var jobs = await _context.Jobs
            .Where(j => jobIds.Contains(j.Id))
            .ToListAsync(cancellationToken);

        var jobById = jobs.ToDictionary(j => j.Id);

        var companyIds = jobs.Select(j => j.CompanyId).Distinct().ToList();

        var companies = await _context.Companies
            .Where(c => companyIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        var companyById = companies.ToDictionary(c => c.Id);

        var now = DateTime.UtcNow;
        var result = new List<MyDropDto>();

        foreach (var sla in slas)
        {
            if (!appById.TryGetValue(sla.ApplicationId, out var application)) continue;

            var jobTitle = "Unknown Job";
            var companyName = "Unknown Company";

            if (jobById.TryGetValue(application.JobId, out var job))
            {
                jobTitle = job.Title;
                if (companyById.TryGetValue(job.CompanyId, out var company))
                {
                    companyName = company.DisplayName;
                }
            }

            result.Add(new MyDropDto(
                sla.ApplicationId,
                jobTitle,
                companyName,
                sla.Status,
                sla.StartedAtUtc,
                sla.DueAtUtc,
                sla.Status == SlaStatus.Pending && sla.DueAtUtc < now));
        }

        return result;
    }
}
