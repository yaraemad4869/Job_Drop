namespace JobDrop.Application.DTOs.Requests;

public record CreateReviewEventRequest(
    Guid ApplicationId,
    string EventType,
    string VisibilityScope = "Internal",
    string? Content = null);
