using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.BasicSalary).HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.Specialization)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Manager)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Nationality)
                .WithMany()
                .HasForeignKey(e => e.NationalityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Country)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Governorate)
                .WithMany(g => g.Employees)
                .HasForeignKey(e => e.GovId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Town)
                .WithMany(t => t.Employees)
                .HasForeignKey(e => e.TownId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Village)
                .WithMany(v => v.Employees)
                .HasForeignKey(e => e.VillageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
