using JobDrop.Domain.Entities.Companies;
using JobDrop.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace JobDrop.Infrastructure.Persistence.Configurations.Companies;

public sealed class OfficeFieldAuditConfiguration : IEntityTypeConfiguration<OfficeFieldAudit>
{
    public void Configure(EntityTypeBuilder<OfficeFieldAudit> builder)
    {
        builder.ToTable("office_field_audits");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.Property(x => x.AuditHash).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(30);
        
        // GIS Mapping: Domain GeoPoint? <-> PostGIS geography (SRID 4326)
        // Using == null instead of 'is null' to avoid expression tree issues
        var geoPointConverter = new ValueConverter<GeoPoint?, string>(
            v => v == null ? "" : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<GeoPoint>(v, (JsonSerializerOptions?)null));

        var geoPointComparer = new ValueComparer<GeoPoint?>(
            (a, b) => a != null && b != null && a.Equals(b),
            v => v != null ? v.GetHashCode() : 0,
            v => v);

        builder.Property(x => x.GpsPoint)
            .HasColumnName("gps_point")
            .HasConversion(geoPointConverter)
            .Metadata.SetValueComparer(geoPointComparer);

        builder.HasIndex(x => x.AuditHash).IsUnique().HasDatabaseName("uq_field_audit_hash");
    }
}
