using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.HasOne(t => t.Governorate)
                .WithMany(g => g.Towns)
                .HasForeignKey(t => t.GovernorateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
