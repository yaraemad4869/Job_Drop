using JobDrop.Domain.Entities.Drops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Drops;

public sealed class DropWalletBalanceConfiguration : IEntityTypeConfiguration<DropWalletBalance>
{
    public void Configure(EntityTypeBuilder<DropWalletBalance> builder)
    {
        builder.ToTable("drop_wallet_balances");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        
        builder.HasIndex(x => x.CandidateProfileId).IsUnique();
        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}
