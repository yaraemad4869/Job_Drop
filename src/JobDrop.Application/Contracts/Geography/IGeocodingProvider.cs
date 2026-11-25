using JobDrop.Domain.ValueObjects;

namespace JobDrop.Application.Contracts.Geography;

public sealed record GeocodingResult(
    string PlaceReferenceId,
    string FormattedAddress,
    GeoPoint Point,
    string? OperatingAreaCode);

public interface IGeocodingProvider
{
    Task<GeocodingResult?> ResolveAsync(string query, string? marketCode, CancellationToken cancellationToken = default);
    Task<GeocodingResult?> ResolvePlaceReferenceAsync(string placeReferenceId, CancellationToken cancellationToken = default);
}