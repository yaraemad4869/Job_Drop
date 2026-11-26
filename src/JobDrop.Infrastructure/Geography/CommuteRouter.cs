using JobDrop.Application.Contracts.Geography;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Infrastructure.Geography;

public sealed class CommuteRouter : ICommuteRouter
{
    public Task<CommuteMatrixResult> GetCommuteMatrixAsync(
        GeoPoint origin,
        IEnumerable<GeoPoint> destinations,
        string mode,
        CancellationToken ct)
    {
        // TODO: Integrate GIS API for actual commute matrix calculation
        var routes = new Dictionary<string, CommuteRoute>();

        foreach (var dest in destinations)
        {
            var key = $"{dest.Latitude},{dest.Longitude}";
            routes[key] = new CommuteRoute(
                DurationSeconds: 0,
                DistanceMeters: 0,
                Status: "Pending"
            );
        }

        return Task.FromResult(new CommuteMatrixResult(routes));
    }
}