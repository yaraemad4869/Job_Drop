using FluentAssertions;
using JobDrop.Domain.ValueObjects;
using Xunit;

namespace JobDrop.Domain.UnitTests.ValueObjects;

public class GeoPointTests
{
    [Fact]
    public void Constructor_WithValidCoordinates_ShouldCreateGeoPoint()
    {
        var point = new GeoPoint(30.0444, 31.2357);
        point.Latitude.Should().Be(30.0444);
        point.Longitude.Should().Be(31.2357);
    }

    [Theory]
    [InlineData(91.0, 0.0)]
    [InlineData(-91.0, 0.0)]
    [InlineData(0.0, 181.0)]
    [InlineData(0.0, -181.0)]
    public void Validate_WithInvalidCoordinates_ShouldThrow(double lat, double lon)
    {
        var point = new GeoPoint(lat, lon);
        var act = () => point.Validate();
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Validate_WithZeroZeroCoordinates_ShouldThrow()
    {
        var point = new GeoPoint(0.0, 0.0);
        var act = () => point.Validate();
        act.Should().Throw<ArgumentException>().WithMessage("Coordinates cannot be (0,0).*");
    }
}
