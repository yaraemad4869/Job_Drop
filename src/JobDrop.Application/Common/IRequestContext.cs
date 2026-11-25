namespace JobDrop.Application.Common;

public interface IRequestContext
{
    string CorrelationId { get; }
    string? IpAddress { get; }
    string? UserAgent { get; }
}