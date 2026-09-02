using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class EntrySettingConfiguration : IEntityTypeConfiguration<EntrySetting>
    {
        public void Configure(EntityTypeBuilder<EntrySetting> builder)
        {
            builder.HasIndex(s => s.Code);
        }
    }
}
