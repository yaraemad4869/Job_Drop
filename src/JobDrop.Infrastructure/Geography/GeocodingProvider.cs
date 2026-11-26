using JobDrop.Application.Contracts.Geography;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Infrastructure.Geography;

public sealed class GeocodingProvider : IGeocodingProvider
{
    public Task<GeoPoint> GeocodeAddressAsync(string address, string marketCode, CancellationToken ct)
    {
        // TODO: Integrate GIS API
        return Task.FromResult(new GeoPoint(0, 0));
    }

    public Task<string> ReverseGeocodeAsync(GeoPoint point, CancellationToken ct)
    {
        // TODO: Integrate GIS API
        return Task.FromResult("Unknown Address");
    }
}
