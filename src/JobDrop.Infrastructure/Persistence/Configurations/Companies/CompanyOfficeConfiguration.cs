using System.Text.Json;
using JobDrop.Domain.Entities.Companies;
using JobDrop.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JobDrop.Infrastructure.Persistence.Configurations.Companies;

public sealed class CompanyOfficeConfiguration : IEntityTypeConfiguration<CompanyOffice>
{
    public void Configure(EntityTypeBuilder<CompanyOffice> builder)
    {
        builder.ToTable("company_offices");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Nickname).IsRequired().HasMaxLength(150);
        builder.Property(x => x.TransitAnchorText).HasMaxLength(250);
        builder.Property(x => x.MakaniCode).HasMaxLength(50);
        builder.Property(x => x.PlusCode).HasMaxLength(50);
        builder.Property(x => x.RowVersion).IsRowVersion();

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

        builder.HasOne(x => x.Company).WithMany(c => c.Offices).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.OperatingArea).WithMany().HasForeignKey(x => x.OperatingAreaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.CompanyId, x.VerificationLevel }).HasDatabaseName("ix_company_offices_company_verification");
    }
}