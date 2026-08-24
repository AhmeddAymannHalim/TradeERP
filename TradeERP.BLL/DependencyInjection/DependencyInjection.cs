using Microsoft.Extensions.DependencyInjection;
using TradeERP.DAL.UnitOfWork;

namespace TradeERP.BLL.DependencyInjection
{
    /// <summary>
    /// Registers BLL-layer and shared infrastructure services (AutoMapper, UnitOfWork,
    /// and every I{Entity}Service / {Entity}Service pair as those modules are added).
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register each entity service pair here as modules are added, e.g.:
            // services.AddScoped<IEmployeeService, EmployeeService>();
            // services.AddScoped<IDepartmentService, DepartmentService>();

            return services;
        }
    }
}
