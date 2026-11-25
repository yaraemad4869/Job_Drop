using System.Text.Json;
using JobDrop.Domain.Entities.Geography;
using JobDrop.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JobDrop.Infrastructure.Persistence.Configurations.Geography;

public sealed class OperatingAreaConfiguration : IEntityTypeConfiguration<OperatingArea>
{
    public void Configure(EntityTypeBuilder<OperatingArea> builder)
    {
        builder.ToTable("operating_areas");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.MarketCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Country).HasMaxLength(100).IsRequired();
        builder.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
        builder.Property(x => x.TimeZoneId).HasMaxLength(64).IsRequired();
        builder.Property(x => x.BoundarySource).HasMaxLength(50);

        // Boundary stored as JSONB (GeoJSON MultiPolygon)
        builder.Property(x => x.BoundaryGeoJson)
            .HasColumnName("boundary_geojson")
            .HasColumnType("jsonb")
            .IsRequired();

        // CenterPoint stored as JSONB
        var geoPointConverter = new ValueConverter<GeoPoint, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<GeoPoint>(v, (JsonSerializerOptions?)null) ?? new GeoPoint(0, 0));

        var geoPointComparer = new ValueComparer<GeoPoint>(
            (a, b) => a != null && b != null && a.Equals(b),
            v => v != null ? v.GetHashCode() : 0,
            v => v);

        builder.Property(x => x.CenterPoint)
            .HasColumnName("center_point")
            .HasColumnType("jsonb")
            .IsRequired()
            .HasConversion(geoPointConverter)
            .Metadata.SetValueComparer(geoPointComparer);

        builder.HasIndex(x => x.MarketCode).IsUnique().HasDatabaseName("uq_operating_areas_market_code");
    }
}