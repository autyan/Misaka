using System;
using Misaka.Config;
using Misaka.DependencyInjection;
using Misaka.DependencyInject.Autofac;

namespace Misaka.MessageQueue.Kafka
{
    public static class ServicesExtension
    {
        public static Configuration UseKafkaMq(this Configuration configuration, Action<KafkaOption> optionSetup = null)
        {
            ObjectProviderFactory.Instance.ObjectProviderBuilder.AddSingleton<IProducer, KafkaProducer>();
            ObjectProviderFactory.Instance.ObjectProviderBuilder.AddSingleton<IConsumer, KafkaConsumer>();
            configuration.MakeSureConfig(optionSetup);

            return configuration;
        }
    }
}
