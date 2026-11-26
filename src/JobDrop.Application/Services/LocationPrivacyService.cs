namespace JobDrop.Application.Services;

/// <summary>
/// BE-17: Handles location precision for internal storage vs. external sharing.
/// </summary>
public class LocationPrivacyService
{
    public (double Lat, double Lon) RoundForSharing(double lat, double lon, int precisionMeters = 500)
    {
        // Approximate rounding: 1 degree lat ~ 111km, 1 degree lon ~ 111km * cos(lat)
        var latDelta = precisionMeters / 111000.0;
        var lonDelta = precisionMeters / (111000.0 * Math.Cos(lat * Math.PI / 180.0));
        
        var roundedLat = Math.Round(lat / latDelta) * latDelta;
        var roundedLon = Math.Round(lon / lonDelta) * lonDelta;
        
        return (roundedLat, roundedLon);
    }

    public bool IsDifferentialPrivacyClaimValid()
    {
        // Do not describe spatial rounding as Differential Privacy without defined mechanism
        return false;
    }
}
