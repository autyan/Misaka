using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Misaka.DpendencyInjection;

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

        public IObjectProviderBuilder RegisterInstance(Type type, object instance)
        {
            _containerBuilder.RegisterInstance(type);

            return this;
        }

        public IObjectProviderBuilder Register<TService>(Func<IObjectProvider, TService> implementationFactory,
                                                         ServiceLifetime lifetime = ServiceLifetime.Transient) where TService : class
        {
            var registration = _containerBuilder.Register(context => implementationFactory.Invoke(Build()));
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    registration.SingleInstance();
                    break;
                case ServiceLifetime.Scoped:
                    registration.InstancePerLifetimeScope();
                    break;
                case ServiceLifetime.Transient:
                    registration.InstancePerDependency();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
            return this;
        }

        public IObjectProviderBuilder Register<TService, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Transient) 
            where TService : class where TImplementation : class, TService
        {
            var registration = _containerBuilder.RegisterType<TService>().As<TImplementation>();
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    registration.SingleInstance();
                    break;
                case ServiceLifetime.Scoped:
                    registration.InstancePerLifetimeScope();
                    break;
                case ServiceLifetime.Transient:
                    registration.InstancePerDependency();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
            return this;
        }

        public ObjectProviderFactory Populate(IServiceCollection services)
        {
            _containerBuilder.Populate(services);
            return ObjectProviderFactory.Instance;
        }
    }
}
