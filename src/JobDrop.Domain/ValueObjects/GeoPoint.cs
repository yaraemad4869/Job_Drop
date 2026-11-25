namespace JobDrop.Domain.ValueObjects;

public sealed record GeoPoint(double Latitude, double Longitude)
{
    public GeoPoint() : this(0.0, 0.0) { }
    
    public void Validate()
    {
        if (Latitude < -90 || Latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(Latitude), "Latitude must be between -90 and 90.");
        if (Longitude < -180 || Longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(Longitude), "Longitude must be between -180 and 180.");
        if (Latitude == 0.0 && Longitude == 0.0)
            throw new ArgumentException("Coordinates cannot be (0,0).");
    }
}
