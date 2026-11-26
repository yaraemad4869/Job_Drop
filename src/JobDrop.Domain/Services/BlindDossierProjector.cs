using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Services;

/// <summary>
/// Projects candidate profile into blind or disclosed dossier
/// based on disclosure stage.
/// </summary>
public sealed class BlindDossierProjector
{
    private readonly CandidateCodeGenerator _codeGenerator;

    public BlindDossierProjector(CandidateCodeGenerator codeGenerator)
    {
        _codeGenerator = codeGenerator;
    }

    /// <summary>
    /// Determines disclosure stage based on application status.
    /// </summary>
    public DisclosureStage DetermineStage(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.InterviewScheduled => DisclosureStage.Disclosed,
            ApplicationStatus.OfferExtended => DisclosureStage.Disclosed,
            ApplicationStatus.Hired => DisclosureStage.Disclosed,
            _ => DisclosureStage.Blind
        };
    }

    /// <summary>
    /// Masks candidate name: "Sara Ahmed" -> "Sara A."
    /// </summary>
    public string MaskName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return "Anonymous";

        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return "Anonymous";
        if (parts.Length == 1) return parts[0];

        return $"{parts[0]} {parts[^1][0]}.";
    }

    /// <summary>
    /// Generates privacy-buffered location description.
    /// Quantizes to ~1.2km neighborhood zone.
    /// </summary>
    public string GetApproximateLocation(double latitude, double longitude)
    {
        // Quantize to ~0.01 degree (~1.1km)
        var quantizedLat = Math.Round(latitude, 2);
        var quantizedLon = Math.Round(longitude, 2);
        return $"Area ({quantizedLat:F2}, {quantizedLon:F2})";
    }
}