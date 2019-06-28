using System;
using Microsoft.Extensions.DependencyInjection;

namespace Misaka.DpendencyInjection
{
    public interface IObjectProviderBuilder
    {
        IObjectProvider Build(IServiceCollection serviceCollection = null);

        IObjectProviderBuilder RegisterInstance(Type type, object instance);

        IObjectProviderBuilder Register<TService>(Func<IObjectProvider, TService> implementationFactory,
                                                  ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class;

        IObjectProviderBuilder Register<TService, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class where TImplementation : class, TService;

        ObjectProviderFactory Populate(IServiceCollection services);
    }
}
