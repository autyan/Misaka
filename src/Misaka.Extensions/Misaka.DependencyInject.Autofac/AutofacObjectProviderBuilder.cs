using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Misaka.DependencyInjection;

namespace Misaka.DependencyInject.Autofac
{
    public class AutofacObjectProviderBuilder : IObjectProviderBuilder
    {
        private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();

        public IObjectProvider Build(IServiceCollection serviceCollection = null)
        {
            if (serviceCollection != null)
            {
                _containerBuilder.Populate(serviceCollection);
            }

            return new AutofacObjectProvider(_containerBuilder.Build());
        }

        public ObjectProviderFactory Populate(IServiceCollection services)
        {
            _containerBuilder.Populate(services);
            return ObjectProviderFactory.Instance;
        }

        public IObjectProviderBuilder RegisterInstance(object instance)
        {
            _containerBuilder.RegisterInstance(instance);
            return this;
        }

        public IObjectProviderBuilder RegisterInstance(Type type, object instance)
        {
            _containerBuilder.RegisterInstance(instance).As(type);
            return this;
        }

        public IObjectProviderBuilder RegisterNamed<T>(string name, ServiceLifetime serviceLifetime) where T : class
        {
            _containerBuilder.RegisterType<T>()
                             .Named<T>(name)
                             .InstancePerLifeTime(serviceLifetime);
            return this;
        }

        public IObjectProviderBuilder Register<T>(ServiceLifetime serviceLifetime) where T : class
        {
            _containerBuilder.RegisterType<T>()
                             .InstancePerLifeTime(serviceLifetime);
            return this;
        }
    }
}
