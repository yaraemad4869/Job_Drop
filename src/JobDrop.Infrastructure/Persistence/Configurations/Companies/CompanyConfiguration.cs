using JobDrop.Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Companies;

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.LegalName).HasMaxLength(250).IsRequired();
        builder.Property(x => x.DisplayName).HasMaxLength(250).IsRequired();
        builder.Property(x => x.ThemeColorHex).HasMaxLength(9);
        builder.Property(x => x.CommercialLicenseNo).HasMaxLength(100);
        builder.Property(x => x.Website).HasMaxLength(500);
        builder.Property(x => x.Description).HasMaxLength(4000);
        builder.Property(x => x.Industry).HasMaxLength(150);
        builder.Property(x => x.RowVersion).IsRowVersion();
        builder.HasIndex(x => x.VerificationLevel);
    }
}