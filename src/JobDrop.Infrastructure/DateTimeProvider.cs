using JobDrop.Application.Common;

namespace JobDrop.Infrastructure;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    
    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);
}
