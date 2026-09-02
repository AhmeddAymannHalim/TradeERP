using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Models;
using TradeERP.Shared.HelperServices.Interfaces;

namespace TradeERP.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Specialization> Specializations => Set<Specialization>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Governorate> Governorates => Set<Governorate>();
        public DbSet<Town> Towns => Set<Town>();
        public DbSet<Village> Villages => Set<Village>();

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<LedgerAccount> LedgerAccounts => Set<LedgerAccount>();
        public DbSet<BillMaster> BillMasters => Set<BillMaster>();
        public DbSet<BillDetails> BillDetails => Set<BillDetails>();
        public DbSet<EntryMaster> EntryMasters => Set<EntryMaster>();
        public DbSet<EntryDetails> EntryDetails => Set<EntryDetails>();
        public DbSet<BillSetting> BillSettings => Set<BillSetting>();
        public DbSet<EntrySetting> EntrySettings => Set<EntrySetting>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                    continue;

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var filter = Expression.Lambda(Expression.Not(property), parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            var userId = _currentUserService.UserId;
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.CreatedAt = now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = userId;
                        entry.Entity.UpdatedAt = now;
                        break;

                    case EntityState.Deleted:
                        // Soft delete: never physically remove a BaseEntity row.
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedBy = userId;
                        entry.Entity.DeletedAt = now;
                        break;
                }
            }
        }
    }
}
