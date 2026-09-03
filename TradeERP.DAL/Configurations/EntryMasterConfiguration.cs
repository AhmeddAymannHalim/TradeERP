using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class EntryMasterConfiguration : IEntityTypeConfiguration<EntryMaster>
    {
        public void Configure(EntityTypeBuilder<EntryMaster> builder)
        {
            builder.HasIndex(e => e.Code);

            builder.HasOne(e => e.SourceBillMaster)
                .WithMany()
                .HasForeignKey(e => e.SourceBillMasterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
