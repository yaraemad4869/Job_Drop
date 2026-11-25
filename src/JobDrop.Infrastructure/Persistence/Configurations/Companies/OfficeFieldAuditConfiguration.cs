using System.Text.Json;
using JobDrop.Domain.Entities.Companies;
using JobDrop.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JobDrop.Infrastructure.Persistence.Configurations.Companies;

public sealed class OfficeFieldAuditConfiguration : IEntityTypeConfiguration<OfficeFieldAudit>
{
    public void Configure(EntityTypeBuilder<OfficeFieldAudit> builder)
    {
        builder.ToTable("office_field_audits");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.AuditHash).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(50).IsRequired();
        builder.Property(x => x.FieldEvidenceKey).HasMaxLength(500);

        // Convert GeoPoint ValueObject to JSONB for PostgreSQL
        var geoPointConverter = new ValueConverter<GeoPoint, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<GeoPoint>(v, (JsonSerializerOptions?)null));

        var geoPointComparer = new ValueComparer<GeoPoint>(
            (a, b) => a != null && b != null && a.Equals(b),
            v => v != null ? v.GetHashCode() : 0,
            v => v);

        builder.Property(x => x.GpsPoint)
            .HasColumnName("gps_point")
            .HasColumnType("jsonb")
            .HasConversion(geoPointConverter)
            .Metadata.SetValueComparer(geoPointComparer);

        builder.HasOne<CompanyOffice>()
            .WithMany()
            .HasForeignKey(x => x.CompanyOfficeId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(x => x.AuditHash).IsUnique().HasDatabaseName("uq_office_field_audits_hash");
    }
}