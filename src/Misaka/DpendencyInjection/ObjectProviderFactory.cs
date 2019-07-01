using System;
using Microsoft.Extensions.DependencyInjection;

namespace Misaka.DpendencyInjection
{
    public class ObjectProviderFactory
    {
        public static ObjectProviderFactory Instance => new ObjectProviderFactory();

        private ObjectProviderFactory()
        {

        }

        #region Members

        private IObjectProviderBuilder _objectProviderBuilder;
        public IObjectProviderBuilder ObjectProviderBuilder
        {
            get
            {
                if (_objectProviderBuilder == null)
                {
                    throw new Exception("Please SetProviderBuilder first.");
                }
                return _objectProviderBuilder;
            }
            set => _objectProviderBuilder = value;
        }

        private IObjectProvider _objectProvider;
        public IObjectProvider ObjectProvider
        {
            get
            {
                if (_objectProvider == null)
                {
                    throw new Exception("Please call Build first.");
                }
                return _objectProvider;
            }
        }

        public IObjectProviderBuilder SetProviderBuilder(IObjectProviderBuilder objectProviderBuilder)
        {
            return ObjectProviderBuilder = objectProviderBuilder;
        }

        public IObjectProvider Build(IServiceCollection serviceCollection = null)
        {
            return _objectProvider = ObjectProviderBuilder.Build(serviceCollection);
        }

        #endregion

        public static IObjectProvider CreateScope()
        {
            return Instance.ObjectProvider.CreateScope();
        }

        public static IObjectProvider CreateScope(IServiceCollection serviceCollection)
        {
            return Instance.ObjectProvider.CreateScope(serviceCollection);
        }

        public static T GetService<T>(string name) where T : class
        {
            return Instance.ObjectProvider.GetService<T>(name);
        }

        public static T GetService<T>() where T : class
        {
            return Instance.ObjectProvider.GetService<T>();
        }

        public static object GetService(Type type)
        {
            return Instance.ObjectProvider.GetService(type);
        }



        public static object GetService(Type type, string name)
        {
            return Instance.ObjectProvider.GetService(type, name);
        }

        public IObjectProviderBuilder RegisterInstance(Type type, object instance)
        {
            ObjectProviderBuilder.RegisterInstance(type, instance);
            return ObjectProviderBuilder;
        }

        public IObjectProviderBuilder RegisterInstance(object instance)
        {
            ObjectProviderBuilder.RegisterInstance(instance.GetType(), instance);
            return ObjectProviderBuilder;
        }

        public IObjectProviderBuilder RegisterType<TService>(Func<IObjectProvider, TService> implementationFactory, 
                                                             ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class
        {
            ObjectProviderBuilder.Register(implementationFactory, lifetime);
            return ObjectProviderBuilder;
        }

        public IObjectProviderBuilder RegisterType<TService, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class where TImplementation : class, TService
        {
            ObjectProviderBuilder.Register<TService, TImplementation>(lifetime);
            return ObjectProviderBuilder;
        }

        public ObjectProviderFactory RegisterComponents(Action<IObjectProviderBuilder, ServiceLifetime> registerComponents,
                                             ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            registerComponents(ObjectProviderBuilder, lifetime);
            return Instance;
        }

        public ObjectProviderFactory Populate(IServiceCollection services)
        {
            ObjectProviderBuilder.Populate(services);
            return this;
        }
    }
}
