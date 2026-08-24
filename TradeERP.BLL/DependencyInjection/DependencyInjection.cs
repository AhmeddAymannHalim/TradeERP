using Microsoft.Extensions.DependencyInjection;
using TradeERP.DAL.UnitOfWork;

namespace TradeERP.BLL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
