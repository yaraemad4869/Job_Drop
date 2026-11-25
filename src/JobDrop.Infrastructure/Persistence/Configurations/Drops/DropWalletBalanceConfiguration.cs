using JobDrop.Domain.Entities.Drops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Drops;

public sealed class DropWalletBalanceConfiguration : IEntityTypeConfiguration<DropWalletBalance>
{
    public void Configure(EntityTypeBuilder<DropWalletBalance> builder)
    {
        builder.ToTable("DropWalletBalances");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.RowVersion).IsRowVersion();
        builder.HasOne(x => x.CandidateProfile).WithMany().HasForeignKey(x => x.CandidateProfileId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.CandidateProfileId).IsUnique();
    }
}