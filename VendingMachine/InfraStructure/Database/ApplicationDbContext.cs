using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VendingMachineAPI.InfraStructure.Configuration;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(VendingMachineCoinsConfig).Assembly);
            base.OnModelCreating(builder);
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<VendingMachineCoins> VendingMachineCoins { get; set; }
    }
}
