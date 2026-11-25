using JobDrop.Application.Contracts.Geography;
using JobDrop.Domain.Enums;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Infrastructure.Geography;

public sealed class InMemoryCommuteRouter : ICommuteRouter
{
    public Task<IReadOnlyList<CommuteRouteResult>> ComputeMatrixAsync(
        IReadOnlyList<GeoPoint> origins,
        IReadOnlyList<GeoPoint> destinations,
        TravelMode mode,
        DateTimeOffset departureTimeUtc,
        CancellationToken cancellationToken = default)
    {
        var results = new List<CommuteRouteResult>();
        foreach (var o in origins)
        {
            foreach (var d in destinations)
            {
                var dist = HaversineMeters(o, d);
                var speed = mode switch
                {
                    TravelMode.Walking => 5.0,
                    TravelMode.Micromobility => 15.0,
                    TravelMode.ScheduledTransit => 25.0,
                    TravelMode.Car => 40.0,
                    _ => 30.0
                };
                var seconds = (int)(dist / speed * 3.6);
                results.Add(new CommuteRouteResult(o, d, mode, seconds, (int)dist, RouteStatus.Available, RouteSource.Live, DateTimeOffset.UtcNow));
            }
        }
        return Task.FromResult<IReadOnlyList<CommuteRouteResult>>(results);
    }

    private static double HaversineMeters(GeoPoint a, GeoPoint b)
    {
        const double R = 6371000;
        var dLat = (b.Latitude - a.Latitude) * Math.PI / 180;
        var dLon = (b.Longitude - a.Longitude) * Math.PI / 180;
        var lat1 = a.Latitude * Math.PI / 180;
        var lat2 = b.Latitude * Math.PI / 180;
        var x = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(x), Math.Sqrt(1 - x));
        return R * c;
    }
}