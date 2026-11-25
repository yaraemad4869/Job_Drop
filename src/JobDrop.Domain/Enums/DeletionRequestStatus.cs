namespace JobDrop.Domain.Enums;

public enum DeletionRequestStatus
{
    Pending = 0,
    Purging = 1,
    Completed = 2,
    Failed = 3
}