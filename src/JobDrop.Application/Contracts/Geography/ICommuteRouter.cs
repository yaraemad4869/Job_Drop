using JobDrop.Domain.ValueObjects;

namespace JobDrop.Application.Contracts.Geography;

public interface ICommuteRouter
{
    Task<CommuteMatrixResult> GetCommuteMatrixAsync(
        GeoPoint origin,
        IEnumerable<GeoPoint> destinations,
        string travelMode,
        CancellationToken cancellationToken);
}

public sealed record CommuteMatrixResult(
    Dictionary<string, CommuteRoute> Routes);

public sealed record CommuteRoute(
    int DurationSeconds,
    int DistanceMeters,
    string Status); // Available, Unreachable, Unsupported
