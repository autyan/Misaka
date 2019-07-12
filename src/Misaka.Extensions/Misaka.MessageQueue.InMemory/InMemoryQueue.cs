using Misaka.DependencyInjection;
using Misaka.Message;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Misaka.MessageQueue.InMemory
{
    public class InMemoryQueue : MessageConsumer, IProducer
    {
        private static readonly Channel<InMemoryMessage> MessageChannel = Channel.CreateUnbounded<InMemoryMessage>();

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
            MessageChannel.Writer.WriteAsync(new InMemoryMessage
                                             {
                                                 Topic   = context.Topic,
                                                 Message = context.Message
                                             }).AsTask().Wait();
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
                                      while (await MessageChannel.Reader.WaitToReadAsync())
                                      {
                                          if (!MessageChannel.Reader.TryRead(out var message)) continue;
                                          if (!Topics.Contains(message.Topic)) continue;

                                          await HandleMessageAsync(() => new MessageHandleContext
                                                                         {
                                                                             Topic   = message.Topic,
                                                                             Message = message.Message
                                                                         });
                                      }
                                  });
        }

        public override void Stop()
        {
            MessageChannel.Writer.Complete();
        }
    }
}
