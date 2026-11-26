using JobDrop.Application.Contracts.Geography;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Infrastructure.Geography;

public sealed class OperatingAreaQuery : IOperatingAreaQuery
{
    public Task<OperatingAreaInfo?> GetByMarketCodeAsync(string marketCode, CancellationToken ct)
    {
        // TODO: Query from database
        return Task.FromResult<OperatingAreaInfo?>(null);
    }

    public Task<IReadOnlyList<OperatingAreaInfo>> ListActiveAsync(string? country, CancellationToken ct)
    {
        // TODO: Query from database
        return Task.FromResult<IReadOnlyList<OperatingAreaInfo>>(Array.Empty<OperatingAreaInfo>());
    }

    public Task<bool> ContainsPointAsync(Guid operatingAreaId, GeoPoint point, CancellationToken ct)
    {
        // TODO: PostGIS query
        return Task.FromResult(true);
    }

    public Task<OperatingAreaInfo?> FindAreaForPointAsync(GeoPoint point, CancellationToken ct)
    {
        // TODO: PostGIS query
        return Task.FromResult<OperatingAreaInfo?>(null);
    }
}
