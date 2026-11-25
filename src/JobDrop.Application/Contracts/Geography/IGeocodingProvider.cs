using JobDrop.Domain.ValueObjects;

namespace JobDrop.Application.Contracts.Geography;

public interface IGeocodingProvider
{
    Task<GeoPoint> GeocodeAddressAsync(string address, string marketCode, CancellationToken cancellationToken);
    Task<string> ReverseGeocodeAsync(GeoPoint location, CancellationToken cancellationToken);
}
