using System;
using System.Collections;
using System.Threading.Tasks;

namespace Misaka.MessageQueue
{
    public interface IMessageBus : IDisposable
    {
        void Publish(object message);

        void Publish(IEnumerable messages);

        Task PublishAsync(object message);
        
        Task PublishAsync(IEnumerable messages);
    }
}
