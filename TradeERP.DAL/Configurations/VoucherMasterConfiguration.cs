using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class VoucherMasterConfiguration : IEntityTypeConfiguration<VoucherMaster>
    {
        public void Configure(EntityTypeBuilder<VoucherMaster> builder)
        {
            builder.HasIndex(v => v.Code).IsUnique();

            builder.Property(v => v.Amount).HasColumnType("decimal(18,2)");

            builder.HasOne(v => v.Customer)
                .WithMany()
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Supplier)
                .WithMany()
                .HasForeignKey(v => v.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.TreasuryLedgerAccount)
                .WithMany()
                .HasForeignKey(v => v.TreasuryLedgerAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
