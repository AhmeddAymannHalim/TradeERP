using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class VillageConfiguration : IEntityTypeConfiguration<Village>
    {
        public void Configure(EntityTypeBuilder<Village> builder)
        {
            builder.HasOne(v => v.Town)
                .WithMany(t => t.Villages)
                .HasForeignKey(v => v.TownId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
