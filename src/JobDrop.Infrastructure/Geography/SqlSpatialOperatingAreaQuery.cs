using JobDrop.Application.Contracts.Geography;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Infrastructure.Geography;

/// <summary>
/// Stub implementation of IOperatingAreaQuery.
/// GIS functionality will be integrated later via external GIS API.
/// Currently provides basic CRUD operations without spatial queries.
/// </summary>
public sealed class SqlSpatialOperatingAreaQuery : IOperatingAreaQuery
{
    private readonly IApplicationDbContext _db;

    public SqlSpatialOperatingAreaQuery(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<OperatingAreaInfo?> GetByMarketCodeAsync(string marketCode, CancellationToken cancellationToken)
    {
        return await _db.OperatingAreas.AsNoTracking()
            .Where(a => a.MarketCode == marketCode && a.IsActive)
            .Select(a => new OperatingAreaInfo(a.Id, a.MarketCode, a.Country, a.NameEn, a.NameAr, a.TimeZoneId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OperatingAreaInfo>> ListActiveAsync(string? country, CancellationToken cancellationToken)
    {
        var q = _db.OperatingAreas.AsNoTracking().Where(a => a.IsActive);
        if (!string.IsNullOrEmpty(country))
            q = q.Where(a => a.Country == country);
        return await q.Select(a => new OperatingAreaInfo(a.Id, a.MarketCode, a.Country, a.NameEn, a.NameAr, a.TimeZoneId))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// TODO: Integrate with external GIS API for point-in-polygon checks.
    /// Currently returns false as spatial queries require GIS API integration.
    /// </summary>
    public Task<bool> ContainsPointAsync(Guid operatingAreaId, GeoPoint point, CancellationToken cancellationToken)
    {
        // TODO: Call external GIS API to check if point is within operating area boundary
        return Task.FromResult(false);
    }

    /// <summary>
    /// TODO: Integrate with external GIS API for reverse geocoding.
    /// Currently returns null as spatial queries require GIS API integration.
    /// </summary>
    public Task<OperatingAreaInfo?> FindAreaForPointAsync(GeoPoint point, CancellationToken cancellationToken)
    {
        // TODO: Call external GIS API to find operating area containing the point
        return Task.FromResult<OperatingAreaInfo?>(null);
    }
}