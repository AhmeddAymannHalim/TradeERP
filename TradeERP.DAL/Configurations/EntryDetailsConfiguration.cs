using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class EntryDetailsConfiguration : IEntityTypeConfiguration<EntryDetails>
    {
        public void Configure(EntityTypeBuilder<EntryDetails> builder)
        {
            builder.HasIndex(d => d.Code);

            builder.Property(d => d.DebitAmount).HasColumnType("decimal(18,2)");
            builder.Property(d => d.CreditAmount).HasColumnType("decimal(18,2)");

            builder.HasOne(d => d.EntryMaster)
                .WithMany(m => m.EntryDetails)
                .HasForeignKey(d => d.EntryMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.LedgerAccount)
                .WithMany(a => a.EntryDetails)
                .HasForeignKey(d => d.LedgerAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
