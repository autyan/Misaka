using System;
using Microsoft.Extensions.DependencyInjection;
using Misaka.Config;
using Misaka.DependencyInject.Autofac;
using Misaka.DependencyInjection;

namespace Misaka.MessageQueue.InMemory
{
    public static class ServiceExtension
    {
        public static Configuration UseInMemoryQueue(this Configuration configuration, Action<ConsumerOption> optionSetup = null)
        {
            ObjectProviderFactory.Instance.ObjectProviderBuilder.AddScoped<IProducer, InMemoryQueue>();
            ObjectProviderFactory.Instance.ObjectProviderBuilder.AddScoped<IConsumer, InMemoryQueue>();
            configuration.MakeSureConfig(optionSetup);
            
            return configuration;
        }
    }
}
