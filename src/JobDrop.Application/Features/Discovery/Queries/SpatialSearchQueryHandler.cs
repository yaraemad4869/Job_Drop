using MediatR;
using JobDrop.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Discovery.Queries;

public sealed record SpatialSearchQuery(
    Guid CandidateProfileId,
    double Latitude,
    double Longitude,
    double RadiusInKm,
    int Page = 1,
    int PageSize = 20) : IRequest<SpatialSearchResult>;

public sealed record SpatialSearchResult(
    int TotalCount,
    List<DiscoveredJobDto> Jobs);

public sealed record DiscoveredJobDto(
    Guid JobId,
    string Title,
    string CompanyName,
    double DistanceInKm,
    double OfficeLatitude,
    double OfficeLongitude);

public sealed class SpatialSearchQueryHandler : IRequestHandler<SpatialSearchQuery, SpatialSearchResult>
{
    private readonly IApplicationDbContext _context;

    public SpatialSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpatialSearchResult> Handle(SpatialSearchQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _context.Jobs
            .Where(j => j.IsActive)
            .ToListAsync(cancellationToken);

        var companyIds = jobs.Select(j => j.CompanyId).Distinct().ToList();

        var offices = await _context.CompanyOffices
            .Where(o => companyIds.Contains(o.CompanyId))
            .ToListAsync(cancellationToken);

        var companies = await _context.Companies
            .Where(c => companyIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        var officeByCompany = offices
            .GroupBy(o => o.CompanyId)
            .ToDictionary(g => g.Key, g => g.First());

        var companyById = companies.ToDictionary(c => c.Id);

        var discovered = new List<DiscoveredJobDto>();

        foreach (var job in jobs)
        {
            if (!officeByCompany.TryGetValue(job.CompanyId, out var office)) continue;

            var distance = CalculateHaversineDistance(
                request.Latitude, request.Longitude,
                office.Latitude, office.Longitude);

            if (distance > request.RadiusInKm) continue;

            var companyName = companyById.TryGetValue(job.CompanyId, out var company)
                ? company.DisplayName
                : "Unknown Company";

            discovered.Add(new DiscoveredJobDto(
                job.Id,
                job.Title,
                companyName,
                Math.Round(distance, 2),
                office.Latitude,
                office.Longitude));
        }

        var ordered = discovered.OrderBy(j => j.DistanceInKm).ToList();

        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;

        var paged = ordered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new SpatialSearchResult(ordered.Count, paged);
    }

    private static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371.0;
        var dLat = (lat2 - lat1) * Math.PI / 180.0;
        var dLon = (lon2 - lon1) * Math.PI / 180.0;

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }
}
