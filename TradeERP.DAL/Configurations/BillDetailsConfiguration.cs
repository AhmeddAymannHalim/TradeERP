using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class BillDetailsConfiguration : IEntityTypeConfiguration<BillDetails>
    {
        public void Configure(EntityTypeBuilder<BillDetails> builder)
        {
            builder.HasIndex(d => d.Code);

            builder.Property(d => d.Quantity).HasColumnType("decimal(18,2)");
            builder.Property(d => d.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(d => d.LineTotal).HasColumnType("decimal(18,2)");

            builder.HasOne(d => d.BillMaster)
                .WithMany(m => m.BillDetails)
                .HasForeignKey(d => d.BillMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
