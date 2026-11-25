using System.Text.Json;
using JobDrop.Application.Contracts.Geography;
using JobDrop.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace JobDrop.Infrastructure.Geography;

public sealed class CachedCommuteRouter : ICommuteRouter
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<CachedCommuteRouter> _logger;

    public CachedCommuteRouter(IConnectionMultiplexer redis, ILogger<CachedCommuteRouter> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    public async Task<CommuteMatrixResult> GetCommuteMatrixAsync(
        GeoPoint origin,
        IEnumerable<GeoPoint> destinations,
        string travelMode,
        CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var routes = new Dictionary<string, CommuteRoute>();

        foreach (var dest in destinations)
        {
            // Cache key based on quantized origin/dest to reduce external API calls
            var cacheKey = $"commute:{origin.Latitude:F3}:{origin.Longitude:F3}:{dest.Latitude:F3}:{dest.Longitude:F3}:{travelMode}";
            
            var cached = await db.StringGetAsync(cacheKey);
            if (cached.HasValue)
            {
                // Explicitly convert RedisValue to string to resolve ambiguity
                var cachedString = (string)cached!;
                var deserialized = JsonSerializer.Deserialize<CommuteRoute>(cachedString);
                if (deserialized is not null)
                {
                    routes[$"{dest.Latitude},{dest.Longitude}"] = deserialized;
                    continue;
                }
            }

            // TODO: Call External GIS API here (e.g., OSRM, Google Maps, Mapbox)
            // For Sprint 2 scaffolding, we return a mock reachable route
            var mockRoute = new CommuteRoute(DurationSeconds: 1200, DistanceMeters: 5000, Status: "Available");
            
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(mockRoute), TimeSpan.FromDays(7));
            routes[$"{dest.Latitude},{dest.Longitude}"] = mockRoute;
        }

        return new CommuteMatrixResult(routes);
    }
}
