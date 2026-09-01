using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.BLL.Services.Commons;
using TradeERP.BLL.Services.Definitions;
using TradeERP.BLL.Validations.Employee;
using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.DAL.UnitOfWork;

namespace TradeERP.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IValidatorRepository<>), typeof(ValidatorRepository<>));
            services.AddScoped(typeof(IValidatorService<>), typeof(ValidatorService<>));

            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<IEmployeeServices, EmployeeServices>();
            services.AddScoped<ISpecializationServices, SpecializationServices>();

            services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();

            return services;
        }
    }
}
