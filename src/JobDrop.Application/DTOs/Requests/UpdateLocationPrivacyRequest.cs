namespace JobDrop.Application.DTOs.Requests;

public record UpdateLocationPrivacyRequest(
    int SharedPrecisionMeters = 500,
    bool AllowResidentialPoint = false,
    bool AllowExactLocation = false);
