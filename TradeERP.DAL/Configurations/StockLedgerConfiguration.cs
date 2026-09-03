using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class StockLedgerConfiguration : IEntityTypeConfiguration<StockLedger>
    {
        public void Configure(EntityTypeBuilder<StockLedger> builder)
        {
            builder.HasIndex(s => s.ProductId);

            builder.Property(s => s.Quantity).HasColumnType("decimal(18,3)");
            builder.Property(s => s.UnitCost).HasColumnType("decimal(18,2)");

            builder.HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
