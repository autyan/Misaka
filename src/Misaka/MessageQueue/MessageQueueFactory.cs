using Misaka.DependencyInjection;
using System.Threading.Tasks;

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
            var consumer = ObjectProviderFactory.Instance.ObjectProvider.GetService<IConsumer>();
            await consumer.StartAsync();
        }
    }
}
