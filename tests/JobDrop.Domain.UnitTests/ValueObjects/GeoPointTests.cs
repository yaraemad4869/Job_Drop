using Xunit;
using FluentAssertions;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Domain.UnitTests.ValueObjects;

public class GeoPointTests
{
    [Fact]
    public void Constructor_ValidCoordinates_ShouldCreatePoint()
    {
        var point = new GeoPoint(30.0444, 31.2357);
        point.Latitude.Should().Be(30.0444);
        point.Longitude.Should().Be(31.2357);
        point.SRID.Should().Be(4326);
    }

    [Theory]
    [InlineData(91.0, 0.0)]
    [InlineData(-91.0, 0.0)]
    [InlineData(0.0, 181.0)]
    [InlineData(0.0, -181.0)]
    public void Constructor_OutOfRangeCoordinates_ShouldThrow(double lat, double lon)
    {
        var act = () => new GeoPoint(lat, lon);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_NullIsland_ShouldThrow()
    {
        var act = () => new GeoPoint(0, 0);
        act.Should().Throw<ArgumentException>().WithMessage("*Null Island*");
    }

    [Theory]
    [InlineData(double.NaN, 0.0)]
    [InlineData(double.PositiveInfinity, 0.0)]
    public void Constructor_NaNOrInfinity_ShouldThrow(double lat, double lon)
    {
        var act = () => new GeoPoint(lat, lon);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_SameCoordinates_ShouldBeEqual()
    {
        var point1 = new GeoPoint(30.0444, 31.2357);
        var point2 = new GeoPoint(30.0444, 31.2357);
        point1.Should().Be(point2);
        (point1 == point2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentCoordinates_ShouldNotBeEqual()
    {
        var point1 = new GeoPoint(30.0444, 31.2357);
        var point2 = new GeoPoint(25.2048, 55.2708);
        point1.Should().NotBe(point2);
        (point1 != point2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_SameCoordinates_ShouldReturnSameHash()
    {
        var point1 = new GeoPoint(30.0444, 31.2357);
        var point2 = new GeoPoint(30.0444, 31.2357);
        point1.GetHashCode().Should().Be(point2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnWktFormat()
    {
        var point = new GeoPoint(30.0444, 31.2357);
        point.ToString().Should().Be("POINT(31.2357 30.0444)");
    }

    [Fact]
    public void Constructor_CustomSrid_ShouldPreserveSrid()
    {
        var point = new GeoPoint(30.0444, 31.2357, 3857);
        point.SRID.Should().Be(3857);
    }
}