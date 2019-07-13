using System;
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
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IRepository, EfCoreRepository>();

            return services;
        }

        public static IServiceCollection UseEfCoreMessageStore<TMessageStore>(this IServiceCollection services) where TMessageStore : DbContext, IMessageStore
        {
            services.AddScoped<IMessageStore, TMessageStore>();
            return services;
        }

        public static IServiceCollection UserDbContextPool<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> builder) where TDbContext : DbContext
        {
            services.AddDbContextPool<TDbContext>(builder);

            return services;
        }
    }
}