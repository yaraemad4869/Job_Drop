namespace JobDrop.Domain.Enums;

public enum DropStatus
{
    Queued = 0,
    Delivered = 1,
    Opened = 2,
    Reviewed = 3,
    Commented = 4,
    Shortlisted = 5,
    InterviewScheduled = 6,
    OfferExtended = 7,
    CounterOffered = 8,
    Rejected = 9,
    Withdrawn = 10,
    Hired = 11,
    ExpiredRefunded = 12,
    Cancelled = 13
}