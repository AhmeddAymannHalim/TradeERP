using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasIndex(c => c.Code);

            builder.HasOne(c => c.LedgerAccount)
                .WithMany()
                .HasForeignKey(c => c.LedgerAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
