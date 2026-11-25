using JobDrop.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Identity;

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AspNetUsers");

        builder.Property(x => x.Handle)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.Handle)
            .IsUnique()
            .HasDatabaseName("UQ_Users_Handle");

        builder.Property(x => x.GoogleSubjectId)
            .HasMaxLength(200);

        // PostgreSQL-compatible filtered index using verbatim string
        builder.HasIndex(x => x.GoogleSubjectId)
            .IsUnique()
            .HasDatabaseName("UQ_Users_GoogleSub")
            .HasFilter(@"""GoogleSubjectId"" IS NOT NULL");

        builder.Property(x => x.TokenVersion)
            .HasDefaultValue(1);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();
    }
}