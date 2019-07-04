using Microsoft.Extensions.DependencyInjection;
using Misaka.Repository;
using Misaka.UnitOfWork;

namespace Misaka.EntityFrameworkCore
{
    public static class ServiceExtension
    {
        public static IServiceCollection UserEfCore(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IRepository, EfCoreRepository>();

            return services;
        }
    }
}