using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.Configuration
{
    public class ProductsConfig : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.AvailableAmount).HasDefaultValue(0);
            builder.Property(e => e.Cost).HasColumnType("money");
            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            builder.Property(e => e.SellerId).HasMaxLength(450);

        }
    }
}
