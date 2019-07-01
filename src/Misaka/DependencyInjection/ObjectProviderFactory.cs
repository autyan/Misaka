using System;
using Microsoft.Extensions.DependencyInjection;
using Misaka.DpendencyInjection;

namespace Misaka.DependencyInjection
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

        public ObjectProviderFactory Populate(IServiceCollection services)
        {
            ObjectProviderBuilder.Populate(services);
            return this;
        }

        public IObjectProviderBuilder RegisterInstance(object instance)
        {
            return ObjectProviderBuilder.RegisterInstance(instance);
        }

        public IObjectProviderBuilder RegisterInstance(Type type, object instance)
        {
            return ObjectProviderBuilder.RegisterInstance(type, instance);
        }
    }
}
