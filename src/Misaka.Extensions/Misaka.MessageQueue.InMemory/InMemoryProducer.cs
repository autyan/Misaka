using System;
using System.Threading.Tasks;

namespace Misaka.MessageQueue.InMemory
{
    public class InMemoryProducer : IProducer
    {
        public void Publish(PublishContext context)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(PublishContext context)
        {
            throw new NotImplementedException();
        }
    }
}
