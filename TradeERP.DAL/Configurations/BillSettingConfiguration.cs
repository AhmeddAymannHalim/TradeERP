using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class BillSettingConfiguration : IEntityTypeConfiguration<BillSetting>
    {
        public void Configure(EntityTypeBuilder<BillSetting> builder)
        {
            builder.HasIndex(s => s.Code);
        }
    }
}
