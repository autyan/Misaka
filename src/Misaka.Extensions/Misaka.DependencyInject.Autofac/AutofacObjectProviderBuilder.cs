using System;
using System.Reflection;
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
                Populate(serviceCollection);
            }

            _containerBuilder.Register<IObjectProvider>(context =>
                                                        {
                                                            var serviceProvider = context.Resolve<IServiceProvider>() as AutofacServiceProvider;
                                                            var componentContextField = typeof(AutofacServiceProvider).GetField("_lifetimeScope",
                                                                                                                                BindingFlags.NonPublic |
                                                                                                                                BindingFlags.Instance);
                                                            if (componentContextField?.GetValue(serviceProvider) is ILifetimeScope lifetimeScope)
                                                            {
                                                                return new AutofacObjectProvider(lifetimeScope);
                                                            }
                                                            throw new Exception("Autofac ServiceProvider not exists!");
                                                        })
                             .InstancePerLifetimeScope();
            var objectProvider = new AutofacObjectProvider();
            objectProvider.SetLifetimeScope(_containerBuilder.Build());
            return objectProvider;
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

        public IObjectProviderBuilder RegisterNamed<TService, TImplementation>(string name, ServiceLifetime serviceLifetime) 
            where TImplementation : TService
        {
            _containerBuilder.RegisterType<TImplementation>()
                             .Named<TImplementation>(name)
                             .As<TService>()
                             .InstancePerLifeTime(serviceLifetime);
            return this;
        }

        public IObjectProviderBuilder Register<TService, TImplementation>(ServiceLifetime serviceLifetime) 
            where TImplementation : TService
        {
            _containerBuilder.RegisterType<TImplementation>()
                             .As<TService>()
                             .InstancePerLifeTime(serviceLifetime);
            return this;
        }
    }
}
