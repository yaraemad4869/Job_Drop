using FluentAssertions;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Enums;
using Xunit;

namespace JobDrop.Domain.UnitTests.Entities.Drops;

public class ApplicationSlaTests
{
    [Fact]
    public void Constructor_ShouldSetDueAtUtcTo48HoursFromStart()
    {
        var startedAt = new DateTime(2026, 11, 25, 10, 0, 0, DateTimeKind.Utc);
        var sla = new ApplicationSla(Guid.NewGuid(), startedAt, 48);
        
        sla.StartedAtUtc.Should().Be(startedAt);
        sla.DueAtUtc.Should().Be(startedAt.AddHours(48));
        sla.Status.Should().Be(SlaStatus.Pending);
    }

    [Fact]
    public void MarkResponded_ShouldUpdateStatusAndTimestamp()
    {
        var sla = new ApplicationSla(Guid.NewGuid(), DateTime.UtcNow, 48);
        sla.MarkResponded();
        
        sla.Status.Should().Be(SlaStatus.Responded);
        sla.RespondedAtUtc.Should().NotBeNull();
        sla.RespondedAtUtc!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MarkExpiredRefunded_ShouldUpdateStatus()
    {
        var sla = new ApplicationSla(Guid.NewGuid(), DateTime.UtcNow, 48);
        sla.MarkExpiredRefunded();
        
        sla.Status.Should().Be(SlaStatus.ExpiredRefunded);
    }
}
