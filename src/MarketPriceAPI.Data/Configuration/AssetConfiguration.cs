using MarketPriceAPI.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceAPI.Data.Configuration
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Symbol).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Description).HasMaxLength(255);
            builder.Property(a => a.TickSize).HasColumnType("decimal(18,5)");
            builder.Property(a => a.Currency).HasMaxLength(10);
            builder.Property(a => a.BaseCurrency).HasMaxLength(10);
        }
    }
}
