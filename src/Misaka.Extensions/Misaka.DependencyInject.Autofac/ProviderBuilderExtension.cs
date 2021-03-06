﻿using Autofac.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Misaka.DependencyInjection;

namespace Misaka.DependencyInject.Autofac
{
    public static class ProviderBuilderExtension
    {
        public static IRegistrationBuilder<TImplementer, TActivatorData, TStyle>
            InstancePerLifeTime<TImplementer, TActivatorData, TStyle>
            (this IRegistrationBuilder<TImplementer, TActivatorData, TStyle> regBuilder,
             ServiceLifetime                                                 serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    return regBuilder.SingleInstance();
                case ServiceLifetime.Scoped:
                    return regBuilder.InstancePerLifetimeScope();
                case ServiceLifetime.Transient:
                    return regBuilder.InstancePerDependency();
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }
        }

        public static IObjectProviderBuilder AddSingleton<T>(this IObjectProviderBuilder builder)
            where T : class
        {
            return builder.Register<T>(ServiceLifetime.Singleton);
        }

        public static IObjectProviderBuilder AddTransient<T>(this IObjectProviderBuilder builder)
            where T : class
        {
            return builder.Register<T>(ServiceLifetime.Transient);
        }

        public static IObjectProviderBuilder AddScoped<T>(this IObjectProviderBuilder builder)
            where T : class
        {
            return builder.Register<T>(ServiceLifetime.Scoped);
        }

        public static IObjectProviderBuilder AddNamedSingleton<T>(this IObjectProviderBuilder builder, string name)
            where T : class
        {
            return builder.RegisterNamed<T>(name, ServiceLifetime.Singleton);
        }

        public static IObjectProviderBuilder AddNamedTransient<T>(this IObjectProviderBuilder builder, string name)
            where T : class
        {
            return builder.RegisterNamed<T>(name, ServiceLifetime.Transient);
        }

        public static IObjectProviderBuilder AddNamedScoped<T>(this IObjectProviderBuilder builder, string name)
            where T : class
        {
            return builder.RegisterNamed<T>(name, ServiceLifetime.Scoped);
        }

        public static IObjectProviderBuilder AddSingleton<TService, TImplementation>(this IObjectProviderBuilder builder)
            where TImplementation : TService
        {
            return builder.Register<TService, TImplementation>(ServiceLifetime.Singleton);
        }

        public static IObjectProviderBuilder AddTransient<TService, TImplementation>(this IObjectProviderBuilder builder)
            where TImplementation : TService
        {
            return builder.Register<TService, TImplementation>(ServiceLifetime.Transient);
        }

        public static IObjectProviderBuilder AddScoped<TService, TImplementation>(this IObjectProviderBuilder builder)
            where TImplementation : TService
        {
            return builder.Register<TService, TImplementation>(ServiceLifetime.Scoped);
        }

        public static IObjectProviderBuilder AddNamedSingleton<TService, TImplementation>(this IObjectProviderBuilder builder, string name)
            where TImplementation : TService
        {
            return builder.RegisterNamed<TService, TImplementation>(name, ServiceLifetime.Singleton);
        }

        public static IObjectProviderBuilder AddNamedTransient<TService, TImplementation>(this IObjectProviderBuilder builder, string name)
            where TImplementation : TService
        {
            return builder.RegisterNamed<TService, TImplementation>(name, ServiceLifetime.Transient);
        }

        public static IObjectProviderBuilder AddNamedScoped<TService, TImplementation>(this IObjectProviderBuilder builder, string name)
            where TImplementation : TService
        {
            return builder.RegisterNamed<TService, TImplementation>(name, ServiceLifetime.Scoped);
        }
    }
}