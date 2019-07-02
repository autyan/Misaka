using Microsoft.Extensions.DependencyInjection;
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Misaka.DependencyInjection;

namespace Misaka.DependencyInject.Autofac
{
    public class AutofacObjectProvider : IObjectProvider
    {
        public AutofacObjectProvider Parent { get; }

        public ILifetimeScope Scope { get; private set; }

        internal AutofacObjectProvider(AutofacObjectProvider parent = null)
        {
            Parent = parent;
        }

        internal void SetLifetimeScope(ILifetimeScope scope)
        {
            Scope = scope;
        }
        public AutofacObjectProvider(ILifetimeScope scope, AutofacObjectProvider parent = null)
            : this(parent)
        {
            SetLifetimeScope(scope);
        }

        public IObjectProvider CreateScope()
        {
            var objectProvider = new AutofacObjectProvider(this);
            var childScope = Scope.BeginLifetimeScope(builder =>
            {
                builder.RegisterInstance<IObjectProvider>(objectProvider);
            });
            objectProvider.SetLifetimeScope(childScope);
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
            objectProvider.SetLifetimeScope(childScope);
            return objectProvider;
        }

        public T GetService<T>(string name) where T : class
        {
            return Scope.ResolveOptionalNamed<T>(name);
        }

        public T GetService<T>() where T : class
        {
            return Scope.ResolveOptional<T>();
        }

        public object GetService(Type type)
        {
            return Scope.ResolveOptional(type);
        }

        public object GetService(Type type, string name)
        {
            Scope.TryResolveNamed(name, type, out var instance);
            return instance;
        }

        public T GetRequiredService<T>(string name) where T : class
        {
            return Scope.ResolveNamed<T>(name);
        }

        public T GetRequiredService<T>() where T : class
        {
            return Scope.Resolve<T>();
        }

        public object GetRequiredService(Type type)
        {
            return Scope.Resolve(type);
        }

        public object GetRequiredService(Type type, string name)
        {
            return Scope.ResolveNamed(name, type);
        }

        public void Dispose()
        {
            Scope.Dispose();
        }
    }
}
