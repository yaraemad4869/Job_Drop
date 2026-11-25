using System.Text.Json;
using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JobDrop.Infrastructure.Persistence.Configurations.Candidates;

public sealed class StartingPointConfiguration : IEntityTypeConfiguration<StartingPoint>
{
    public void Configure(EntityTypeBuilder<StartingPoint> builder)
    {
        builder.ToTable("starting_points");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Label).HasMaxLength(150).IsRequired();
        builder.Property(x => x.PrivacyZoneLabel).HasMaxLength(150);
        builder.Property(x => x.PlaceReferenceId).HasMaxLength(200);

        // Convert GeoPoint ValueObject to JSONB for PostgreSQL
        var geoPointConverter = new ValueConverter<GeoPoint, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<GeoPoint>(v, (JsonSerializerOptions?)null) ?? new GeoPoint(0, 0));

        var geoPointComparer = new ValueComparer<GeoPoint>(
            (a, b) => a != null && b != null && a.Equals(b),
            v => v != null ? v.GetHashCode() : 0,
            v => v);

        builder.Property(x => x.Location)
            .HasColumnName("location")
            .HasColumnType("jsonb")
            .IsRequired()
            .HasConversion(geoPointConverter)
            .Metadata.SetValueComparer(geoPointComparer);

        builder.HasIndex(x => x.CandidateProfileId).HasDatabaseName("ix_starting_points_candidate");
    }
}