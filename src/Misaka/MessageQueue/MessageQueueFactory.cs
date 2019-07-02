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
            using (var scope = ObjectProviderFactory.Instance.ObjectProvider.CreateScope())
            {
                var consumers = scope.GetService<IEnumerable<IConsumer>>();
                foreach (var consumer in consumers)
                {
                    await consumer.StartAsync()
                                  .ConfigureAwait(false);
                }
            }
        }
    }
}
