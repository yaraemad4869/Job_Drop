namespace JobDrop.Domain.Enums;

/// <summary>
/// Progressive disclosure stages for blind screening.
/// </summary>
public enum DisclosureStage
{
    /// <summary>
    /// Anonymous dossier (#JD-XXXX) with redacted CV.
    /// No PII exposed: no name, photo, phone, exact location.
    /// </summary>
    Blind = 0,
    
    /// <summary>
    /// Full identity disclosed after Interview/Offer.
    /// Name, photo, contact info, exact location revealed.
    /// </summary>
    Disclosed = 1
}