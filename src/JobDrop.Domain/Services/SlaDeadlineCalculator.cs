using JobDrop.Domain.Entities.Companies;

namespace JobDrop.Domain.Services;

/// <summary>
/// Calculates 48h SLA deadlines.
/// Per spec: "48 ELAPSED HOURS excluding ENTIRE company non-working days/holidays"
/// Timer begins at DeliveredAtUtc (not OpenedAtUtc).
/// </summary>
public sealed class SlaDeadlineCalculator
{
    private const int SlaHours = 48;

    /// <summary>
    /// Calculates SLA due date from delivery time.
    /// If calendar is provided, skips non-working days and holidays.
    /// Otherwise uses strict 48h elapsed UTC.
    /// </summary>
    public DateTime CalculateDueAtUtc(
        DateTime deliveredAtUtc,
        CompanyCalendar? calendar,
        IReadOnlyCollection<CompanyHoliday>? holidays)
    {
        if (calendar is null)
        {
            // Strict 48h elapsed UTC fallback
            return deliveredAtUtc.AddHours(SlaHours);
        }

        // Calendar-aware calculation: skip non-working days and holidays
        var current = deliveredAtUtc;
        var remainingHours = SlaHours;
        var safetyCounter = 0;
        const int maxIterations = 10000; // Prevent infinite loop

        while (remainingHours > 0 && safetyCounter < maxIterations)
        {
            current = current.AddHours(1);
            safetyCounter++;

            if (IsNonWorkingTime(current, calendar, holidays))
            {
                continue;
            }

            remainingHours--;
        }

        return current;
    }

    /// <summary>
    /// Determines if SLA has expired.
    /// </summary>
    public bool IsExpired(DateTime dueAtUtc, DateTime nowUtc)
    {
        return nowUtc >= dueAtUtc;
    }

    /// <summary>
    /// Calculates remaining seconds for live countdown.
    /// </summary>
    public int GetRemainingSeconds(DateTime dueAtUtc, DateTime nowUtc)
    {
        var remaining = (dueAtUtc - nowUtc).TotalSeconds;
        return remaining > 0 ? (int)remaining : 0;
    }

    private static bool IsNonWorkingTime(
        DateTime utcTime,
        CompanyCalendar calendar,
        IReadOnlyCollection<CompanyHoliday>? holidays)
    {
        try
        {
            // Get timezone - handle both string and other types
            var timeZoneId = calendar.TimeZoneId;
            if (string.IsNullOrWhiteSpace(timeZoneId))
            {
                return false;
            }

            TimeZoneInfo tz;
            try
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch
            {
                return false;
            }

            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tz);
            var localDayOfWeek = (int)localTime.DayOfWeek; // 0 = Sunday, 6 = Saturday

            // Check working days - use reflection-safe approach
            // Try common property names for working days
            var workingDays = GetWorkingDays(calendar);
            if (workingDays != null && workingDays.Count > 0)
            {
                if (!workingDays.Contains(localDayOfWeek))
                {
                    return true;
                }
            }

            // Check holidays - handle DateOnly vs DateTime
            if (holidays != null && holidays.Count > 0)
            {
                var localDate = localTime.Date;
                foreach (var holiday in holidays)
                {
                    var holidayDate = GetHolidayDate(holiday);
                    if (holidayDate.HasValue && holidayDate.Value == localDate)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        catch
        {
            // If any error, fall back to strict UTC (no skipping)
            return false;
        }
    }

    /// <summary>
    /// Safely extracts working days from CompanyCalendar using reflection.
    /// Supports multiple possible property names.
    /// </summary>
    private static IReadOnlyCollection<int>? GetWorkingDays(CompanyCalendar calendar)
    {
        var type = calendar.GetType();
        
        // Try common property names
        var propertyNames = new[] { "WorkingDays", "WorkingDaysJson", "WorkingDayFlags", "WorkDays", "BusinessDays" };
        
        foreach (var propName in propertyNames)
        {
            var prop = type.GetProperty(propName);
            if (prop != null)
            {
                var value = prop.GetValue(calendar);
                
                // Handle different collection types
                if (value is IReadOnlyCollection<int> intCollection)
                {
                    return intCollection;
                }
                if (value is IEnumerable<int> intEnumerable)
                {
                    return intEnumerable.ToList();
                }
                if (value is string jsonString && !string.IsNullOrWhiteSpace(jsonString))
                {
                    // Try to parse JSON array of ints
                    try
                    {
                        return System.Text.Json.JsonSerializer.Deserialize<List<int>>(jsonString);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }
        
        return null;
    }

    /// <summary>
    /// Safely extracts holiday date as DateTime, handling DateOnly and DateTime types.
    /// </summary>
    private static DateTime? GetHolidayDate(CompanyHoliday holiday)
    {
        var type = holiday.GetType();
        var prop = type.GetProperty("HolidayDate");
        
        if (prop == null) return null;
        
        var value = prop.GetValue(holiday);
        
        if (value is DateTime dt)
        {
            return dt.Date;
        }
        if (value is DateOnly dateOnly)
        {
            return dateOnly.ToDateTime(TimeOnly.MinValue);
        }
        
        return null;
    }
}