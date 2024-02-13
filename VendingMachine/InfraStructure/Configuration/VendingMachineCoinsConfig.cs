using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.Configuration
{
    public class VendingMachineCoinsConfig : IEntityTypeConfiguration<VendingMachineCoins>
    {
        public void Configure(EntityTypeBuilder<VendingMachineCoins> builder)
        {
            
            builder.HasKey(e => e.Coin);
            builder.Property(e => e.Coin).ValueGeneratedNever();
            builder.Property(e => e.AvailableAmount).HasDefaultValue(0);
            
        }
    }
}
