using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Misaka.Config;
using Misaka.DependencyInjection;
using Misaka.MessageStore;
using Misaka.Repository;
using Misaka.UnitOfWork;

namespace Misaka.EntityFrameworkCore
{
    public static class ServiceExtension
    {
        public static Configuration UseEfCore(this Configuration config)
        {
            var services = new ServiceCollection();
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IRepository, EfCoreRepository>();
            ObjectProviderFactory.Instance.ObjectProviderBuilder.Populate(services);
            return config;
        }

        public static Configuration UseEfCoreMessageStore<TMessageStore>(this Configuration config) where TMessageStore : DbContext, IMessageStore
        {
            var services = new ServiceCollection();
            services.AddScoped<IMessageStore, TMessageStore>();
            ObjectProviderFactory.Instance.ObjectProviderBuilder.Populate(services);

            return config;
        }

        public static Configuration UserDbContextPool<TDbContext>(this Configuration config, Action<DbContextOptionsBuilder> builder) where TDbContext : DbContext
        {
            var services = new ServiceCollection();
            services.AddDbContextPool<TDbContext>(builder);
            ObjectProviderFactory.Instance.ObjectProviderBuilder.Populate(services);

            return config;
        }
    }
}