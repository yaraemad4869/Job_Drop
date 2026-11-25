using JobDrop.Domain.ValueObjects;

namespace JobDrop.Application.Contracts.Geography;

public sealed record OperatingAreaInfo(
    Guid Id,
    string MarketCode,
    string Country,
    string NameEn,
    string NameAr,
    string TimeZoneId);

public interface IOperatingAreaQuery
{
    Task<OperatingAreaInfo?> GetByMarketCodeAsync(string marketCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OperatingAreaInfo>> ListActiveAsync(string? country, CancellationToken cancellationToken = default);
    Task<bool> ContainsPointAsync(Guid operatingAreaId, GeoPoint point, CancellationToken cancellationToken = default);
    Task<OperatingAreaInfo?> FindAreaForPointAsync(GeoPoint point, CancellationToken cancellationToken = default);
}