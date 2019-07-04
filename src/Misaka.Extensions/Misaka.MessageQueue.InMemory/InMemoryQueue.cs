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
                             IObjectProvider        objectProvider,
                             ConsumerOption         option) : base(provider, objectProvider, option)
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

        public override void Start()
        {
            StartConsume();
        }

        public override Task StartAsync()
        {
            StartConsume();
            return Task.CompletedTask;
        }

        private void StartConsume()
        {
            Task.Factory.StartNew(async () =>
                                  {
                                      while (true)
                                      {
                                          if (MessageQueue.TryDequeue(out var message))
                                          {
                                              if (!Topics.Contains(message.Topic)) continue;

                                              await HandleMessageAsync(() => new MessageHandleContext
                                              {
                                                  Topic = message.Topic,
                                                  Message = message.Message
                                              });
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
