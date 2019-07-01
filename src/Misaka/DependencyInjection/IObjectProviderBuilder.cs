using System;
using Microsoft.Extensions.DependencyInjection;
using Misaka.DpendencyInjection;

namespace Misaka.DependencyInjection
{
    public interface IObjectProviderBuilder
    {
        IObjectProvider Build(IServiceCollection serviceCollection = null);

        ObjectProviderFactory Populate(IServiceCollection services);

        IObjectProviderBuilder RegisterInstance(object instance);

        IObjectProviderBuilder RegisterInstance(Type type, object instance);

        IObjectProviderBuilder RegisterNamed<T>(string name, ServiceLifetime serviceLifetime) where T : class;

        IObjectProviderBuilder Register<T>(ServiceLifetime serviceLifetime) where T : class;
    }
}
