using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Misaka.MessageStore;
using Misaka.Repository;
using Misaka.UnitOfWork;

namespace Misaka.EntityFrameworkCore
{
    public static class ServiceExtension
    {
        public static IServiceCollection UseEfCore(this IServiceCollection services)
        {
            RegisterEfCoreComponents(services);

            return services;
        }

        public static IServiceCollection UseEfCoreWithMessageStore<TMessageStore>(this IServiceCollection services) where TMessageStore : DbContext, IMessageStore
        {
            RegisterEfCoreComponents(services);
            services.AddScoped<IMessageStore, TMessageStore>();
            return services;
        }

        private static void RegisterEfCoreComponents(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IRepository, EfCoreRepository>();
        }
    }
}