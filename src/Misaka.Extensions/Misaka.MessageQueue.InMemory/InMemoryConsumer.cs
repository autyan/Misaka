using Misaka.DependencyInjection;
using Misaka.Message;
using System;
using System.Threading.Tasks;

namespace Misaka.MessageQueue.InMemory
{
    public class InMemoryConsumer : MessageConsumer
    {
        public InMemoryConsumer(MessageHandlerProvider provider, IObjectProvider objectProvider) 
            : base(provider, objectProvider)
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
    }
}
