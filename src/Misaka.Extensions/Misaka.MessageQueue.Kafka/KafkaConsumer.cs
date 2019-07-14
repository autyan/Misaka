using Microsoft.Extensions.Options;
using Misaka.DependencyInjection;
using Misaka.Message;
using Misaka.MessageStore;
using System;
using System.Threading.Tasks;

namespace Misaka.MessageQueue.Kafka
{
    public class KafkaConsumer : MessageConsumer
    {
        public KafkaConsumer(MessageHandlerProvider          provider,
                             IObjectProvider                 objectProvider,
                             IOptionsMonitor<ConsumerOption> option,
                             IMessageStore                   messageStore)
            : base(provider, objectProvider, option, messageStore)
        {
        }

        public KafkaConsumer(MessageHandlerProvider          provider,
                             IObjectProvider                 objectProvider,
                             IOptionsMonitor<ConsumerOption> option)
            : base(provider, objectProvider, option)
        {
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override Task StartAsync()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
