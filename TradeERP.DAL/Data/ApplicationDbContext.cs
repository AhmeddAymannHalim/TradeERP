using Microsoft.EntityFrameworkCore;

namespace TradeERP.DAL.Data
{
    /// <summary>
    /// Code-first EF Core context. No entities registered yet — DbSet properties
    /// will be added here as each real module (Employee, Department, Product, ...)
    /// is introduced.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
