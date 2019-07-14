using System;
using System.Threading.Tasks;

namespace Misaka.MessageQueue
{
    public interface IProducer : IDisposable
    {
        void Publish(PublishContext context);

        Task PublishAsync(PublishContext context);

        string Name { get; }
    }
}
