using System.Collections.Generic;
using System.Threading.Tasks;
using Misaka.DependencyInjection;

namespace Misaka.MessageQueue
{
    public class MessageQueueFactory
    {
        public static MessageQueueFactory Instance { get; } = new MessageQueueFactory();

        private MessageQueueFactory()
        {

        }

        public async Task StartAsync()
        {
            var consumers = ObjectProviderFactory.Instance.ObjectProvider.GetService<IEnumerable<IConsumer>>();
            foreach (var consumer in consumers)
            {
                await consumer.StartAsync()
                              .ConfigureAwait(false);
            }
        }

        public static void SetConsumerOption(ConsumerOption option)
        {
            ObjectProviderFactory.Instance.ObjectProviderBuilder.RegisterInstance(option);
        }
    }
}
