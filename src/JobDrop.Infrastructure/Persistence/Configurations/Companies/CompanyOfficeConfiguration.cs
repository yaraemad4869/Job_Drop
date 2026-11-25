using JobDrop.Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Companies;

public sealed class CompanyOfficeConfiguration : IEntityTypeConfiguration<CompanyOffice>
{
    public void Configure(EntityTypeBuilder<CompanyOffice> builder)
    {
        builder.ToTable("company_offices");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.Property(x => x.Nickname).IsRequired().HasMaxLength(150);
        builder.Property(x => x.OfficeType).IsRequired().HasMaxLength(30);
        builder.Property(x => x.TransitAnchorText).HasMaxLength(250);
        builder.Property(x => x.GeofenceRadiusMeters).HasDefaultValue(500);
        builder.Property(x => x.LocationVersion).IsConcurrencyToken();

        builder.Property(x => x.Latitude).HasPrecision(9, 6).IsRequired();
        builder.Property(x => x.Longitude).HasPrecision(9, 6).IsRequired();

        builder.HasIndex(x => x.CompanyId);
    }
}
