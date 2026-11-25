namespace JobDrop.Domain.ValueObjects;

public sealed class GeoPoint : ValueObject
{
    public const int Wgs84Srid = 4326;
    public const double MinLatitude = -90.0;
    public const double MaxLatitude = 90.0;
    public const double MinLongitude = -180.0;
    public const double MaxLongitude = 180.0;

    public double Latitude { get; }
    public double Longitude { get; }
    public int SRID { get; }

    public GeoPoint(double latitude, double longitude, int srid = Wgs84Srid)
    {
        if (double.IsNaN(latitude) || double.IsInfinity(latitude))
            throw new ArgumentException("Latitude cannot be NaN or Infinity.", nameof(latitude));
        if (double.IsNaN(longitude) || double.IsInfinity(longitude))
            throw new ArgumentException("Longitude cannot be NaN or Infinity.", nameof(longitude));
        if (latitude < MinLatitude || latitude > MaxLatitude)
            throw new ArgumentOutOfRangeException(nameof(latitude), $"Latitude must be between {MinLatitude} and {MaxLatitude}.");
        if (longitude < MinLongitude || longitude > MaxLongitude)
            throw new ArgumentOutOfRangeException(nameof(longitude), $"Longitude must be between {MinLongitude} and {MaxLongitude}.");
        if (latitude == 0 && longitude == 0)
            throw new ArgumentException("Null Island (0,0) coordinates are not allowed.");

        Latitude = latitude;
        Longitude = longitude;
        SRID = srid;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
        yield return SRID;
    }

    public override string ToString() => $"POINT({Longitude} {Latitude})";
}