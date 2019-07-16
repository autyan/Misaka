using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Misaka.DependencyInjection;
using Misaka.Message;
using Misaka.MessageQueue;

namespace Misaka.Config
{
    public static class ConfigurationExtension
    {
        public static Configuration MakeSureConfig<T>(this Configuration config, Action<T> optionSetup) where T : class
        {
            var services = new ServiceCollection() as IServiceCollection;
            if (optionSetup == null)
            {
                var section = config.GetSection(nameof(ConsumerOption));
                services.Configure<ConsumerOption>(section ?? config.ConfigurationCore);
            }
            else
            {
                services.Configure(optionSetup);
            }
            
            ObjectProviderFactory.Instance.ObjectProviderBuilder.Populate(services);
            return config;
        }
        
        public static Configuration LoadComponent(this Configuration config, Assembly[] assemblies)
        {
            TypeProvider.Instance.LoadFromAssemblies(assemblies);
            return config;
        }

        public static Configuration LoadComponent(this Configuration config, string prefix)
        {
            var files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? throw new InvalidOperationException());
            var validFiles = files.Where(f => f.EndsWith(".dll")).Select(Path.GetFileNameWithoutExtension).Where(n => n.StartsWith(prefix));
            TypeProvider.Instance.LoadFromAssemblies(validFiles.Select(Assembly.Load).ToArray());

            UseMessageQueue();
            return config;
        }

        private static void UseMessageQueue()
        {
            ObjectProviderFactory.Instance.ObjectProviderBuilder.Register<IMessageBus, MessageBus>(ServiceLifetime.Scoped);
            ObjectProviderFactory.Instance.RegisterInstance(typeof(MessageHandlerProvider), new MessageHandlerProvider());

            var handlers = TypeProvider.Instance.FindTypeInfos(info =>
                                                               {
                                                                   return info.GetInterfaces()
                                                                              .Any(i => i.IsGenericType
                                                                                     && i.GetGenericTypeDefinition() ==
                                                                                        typeof(IAsyncMessageSubscriber<>
                                                                                        ));
                                                               });

            foreach (var handler in handlers)
            {
                ObjectProviderFactory.Instance.ObjectProviderBuilder.Register(handler, ServiceLifetime.Scoped);
            }
        }
    }
}
