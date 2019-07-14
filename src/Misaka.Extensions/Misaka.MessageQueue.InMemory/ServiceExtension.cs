using Misaka.Config;
using Misaka.DependencyInject.Autofac;
using Misaka.DependencyInjection;
using System;

namespace Misaka.MessageQueue.InMemory
{
    public static class ServiceExtension
    {
        public static Configuration UseInMemoryQueue(this Configuration configuration, Action<ConsumerOption> optionSetup = null)
        {
            ObjectProviderFactory.Instance.ObjectProviderBuilder.AddSingleton<IProducer, InMemoryQueue>();
            ObjectProviderFactory.Instance.ObjectProviderBuilder.AddSingleton<IConsumer, InMemoryQueue>();
            configuration.MakeSureConfig(optionSetup);
            
            return configuration;
        }
    }
}
