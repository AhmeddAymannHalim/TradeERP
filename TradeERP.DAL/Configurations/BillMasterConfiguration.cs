using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class BillMasterConfiguration : IEntityTypeConfiguration<BillMaster>
    {
        public void Configure(EntityTypeBuilder<BillMaster> builder)
        {
            builder.HasIndex(b => b.Code);

            builder.Property(b => b.Amount).HasColumnType("decimal(18,2)");

            builder.HasOne(b => b.Customer)
                .WithMany()
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Supplier)
                .WithMany()
                .HasForeignKey(b => b.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
