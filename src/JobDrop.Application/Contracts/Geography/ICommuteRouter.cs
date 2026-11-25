using JobDrop.Domain.Enums;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Application.Contracts.Geography;

public sealed record CommuteRouteResult(
    GeoPoint Origin,
    GeoPoint Destination,
    TravelMode Mode,
    int DurationSeconds,
    int DistanceMeters,
    RouteStatus Status,
    RouteSource Source,
    DateTimeOffset AsOfUtc);

public enum RouteStatus { Available, Unreachable, Unsupported, Pending, Stale }
public enum RouteSource { Live, Schedule, Cache }

public interface ICommuteRouter
{
    Task<IReadOnlyList<CommuteRouteResult>> ComputeMatrixAsync(
        IReadOnlyList<GeoPoint> origins,
        IReadOnlyList<GeoPoint> destinations,
        TravelMode mode,
        DateTimeOffset departureTimeUtc,
        CancellationToken cancellationToken = default);
}