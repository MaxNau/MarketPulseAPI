using MarketPriceAPI.Data.Configuration;
using MarketPriceAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceAPI.Data.Contexts
{
    public class MarketPulseAPIDbContext : DbContext
    {
        public MarketPulseAPIDbContext(DbContextOptions<MarketPulseAPIDbContext> options) : base(options) { }

        public DbSet<Asset> Assets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AssetConfiguration());
        }
    }
}
