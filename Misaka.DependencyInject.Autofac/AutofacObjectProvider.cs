using Microsoft.Extensions.DependencyInjection;
using Misaka.DpendencyInjection;
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Misaka.DependencyInject.Autofac
{
    public class AutofacObjectProvider : IObjectProvider
    {
        public AutofacObjectProvider Parent { get; }

        private IComponentContext _componentContext;
        public  ILifetimeScope    Scope => _componentContext as ILifetimeScope;

        internal AutofacObjectProvider(AutofacObjectProvider parent = null)
        {
            Parent = parent;
        }

        internal void SetComponentContext(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }
        public AutofacObjectProvider(IComponentContext componentContext, AutofacObjectProvider parent = null)
            : this(parent)
        {
            SetComponentContext(componentContext);
        }

        public IObjectProvider CreateScope()
        {
            var objectProvider = new AutofacObjectProvider(this);
            var childScope = Scope.BeginLifetimeScope(builder =>
                                                      {
                                                          builder.RegisterInstance<IObjectProvider>(objectProvider);
                                                      });
            objectProvider.SetComponentContext(childScope);
            return objectProvider;
        }

        public IObjectProvider CreateScope(IServiceCollection serviceCollection)
        {
            var objectProvider = new AutofacObjectProvider(this);
            var childScope = Scope.BeginLifetimeScope(builder =>
                                                      {
                                                          builder.RegisterInstance<IObjectProvider>(objectProvider);
                                                          builder.Populate(serviceCollection);
                                                      });
            objectProvider.SetComponentContext(childScope);
            return objectProvider;
        }

        public T GetService<T>(string name) where T : class
        {
            return _componentContext.ResolveOptionalNamed<T>(name);
        }

        public T GetService<T>() where T : class
        {
            return _componentContext.ResolveOptional<T>();
        }

        public object GetService(Type type)
        {
            return _componentContext.ResolveOptional(type);
        }

        public object GetService(Type type, string name)
        {
            _componentContext.TryResolveNamed(name, type, out var instance);
            return instance;
        }
    }
}
