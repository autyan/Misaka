using Misaka.DependencyInjection;
using Misaka.Message;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Misaka.MessageQueue.InMemory
{
    public class InMemoryQueue : MessageConsumer, IProducer
    {
        private static readonly ConcurrentQueue<InMemoryMessage> MessageQueue = new ConcurrentQueue<InMemoryMessage>();

        public InMemoryQueue(MessageHandlerProvider provider, 
                             IObjectProvider objectProvider) 
            : base(provider, objectProvider)
        {
        }

        public void Publish(PublishContext context)
        {
            DoPublish(context);
        }

        public Task PublishAsync(PublishContext context)
        {
            DoPublish(context);
            return Task.CompletedTask;
        }

        private void DoPublish(PublishContext context)
        {
            MessageQueue.Enqueue(new InMemoryMessage
                                 {
                                     Topic = context.Topic,
                                     Message = context.Message
                                 });
        }

        public override void Start(ConsumerOption option)
        {
            StartConsume(option);
        }

        public override Task StartAsync(ConsumerOption option)
        {
            StartConsume(option);
            return Task.CompletedTask;
        }

        private void StartConsume(ConsumerOption option)
        {
            Task.Factory.StartNew(async () =>
                                  {
                                      var consumedTopics = option.Topics.Split(',');
                                      while (true)
                                      {
                                          if (MessageQueue.TryDequeue(out var message))
                                          {
                                              if (!consumedTopics.Contains(message.Topic)) continue;

                                              var context = new MessageHandleContext
                                                            {
                                                                Topic       = message.Topic,
                                                                Message     = message.Message
                                                            };
                                              await HandleMessageAsync(() => context);
                                          }
                                          Thread.Sleep(100);
                                      }
                                      // ReSharper disable once FunctionNeverReturns
                                  });
        }

        public override void Stop()
        {
            
        }
    }
}
