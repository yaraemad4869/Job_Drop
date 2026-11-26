namespace JobDrop.Domain.Enums;

/// <summary>
/// Application lifecycle states per JobDrop specification.
/// </summary>
public enum ApplicationStatus
{
    // Initial states
    Draft = 0,
    Queued = 1,
    Delivered = 2,
    
    // Review states
    Opened = 10,
    Reviewed = 11,
    Commented = 12,
    
    // Decision states
    Shortlisted = 20,
    InterviewScheduled = 21,
    Assessment = 22,
    OfferExtended = 23,
    CounterOffered = 24,
    Hired = 25,
    
    // Rejection states
    Rejected = 30,
    Withdrawn = 31,
    
    // Expiry states
    ExpiredRefunded = 40,
    Cancelled = 41,
    
    // Terminal states
    Closed = 50
}