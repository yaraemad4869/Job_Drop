namespace JobDrop.Application.DTOs.Requests;

public record SetDailyIntakeScheduleRequest(
    Guid JobId,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    int CapLimit,
    string TimeZone = "UTC");
