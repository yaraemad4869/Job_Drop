using JobDrop.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Identity;

public sealed class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.RefreshTokenHash).IsRequired().HasMaxLength(256);
        builder.Property(x => x.DeviceInfo).HasMaxLength(500);
        builder.Property(x => x.IpAddress).HasMaxLength(64);
        builder.HasIndex(x => x.RefreshTokenHash).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.RevokedAtUtc })
            .HasDatabaseName("IX_UserSessions_User_Active");
    }
}