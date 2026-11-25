using System;
using System.Collections.Generic;

namespace JobDrop.Domain.Services;

public sealed class SlaDeadlineCalculator
{
    public const int SlaHours = 48;

    public DateTimeOffset ComputeDeadline(
        DateTimeOffset startUtc,
        string timeZoneId,
        IReadOnlyCollection<DayOfWeek> workingDays,
        IReadOnlyCollection<DateOnly> holidays,
        int workStartMinute,
        int workEndMinute)
    {
        try
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var startLocal = TimeZoneInfo.ConvertTime(startUtc, tz);
            
            // Use DateTime for local arithmetic to avoid DateTimeOffset constructor issues
            DateTime cursor = startLocal.DateTime;
            var remaining = TimeSpan.FromHours(SlaHours);

            int safety = 0;
            while (remaining > TimeSpan.Zero && safety++ < 500)
            {
                var dayStart = cursor.Date;
                var dow = dayStart.DayOfWeek;
                var localDate = DateOnly.FromDateTime(dayStart);
                
                if (!workingDays.Contains(dow) || holidays.Contains(localDate))
                {
                    cursor = dayStart.AddDays(1).AddMinutes(workStartMinute);
                    continue;
                }

                var workStart = dayStart.AddMinutes(workStartMinute);
                var workEnd = dayStart.AddMinutes(workEndMinute);

                if (cursor < workStart) cursor = workStart;
                if (cursor >= workEnd)
                {
                    cursor = dayStart.AddDays(1).AddMinutes(workStartMinute);
                    continue;
                }

                var availableToday = workEnd - cursor;
                if (availableToday >= remaining)
                {
                    cursor = cursor.Add(remaining);
                    remaining = TimeSpan.Zero;
                }
                else
                {
                    remaining -= availableToday;
                    cursor = dayStart.AddDays(1).AddMinutes(workStartMinute);
                }
            }

            // Safely convert back to DateTimeOffset with the correct timezone offset
            var offset = tz.GetUtcOffset(cursor);
            return new DateTimeOffset(cursor, offset).ToUniversalTime();
        }
        catch
        {
            return startUtc.AddHours(SlaHours);
        }
    }
}