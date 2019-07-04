using System.Collections;
using System.Threading.Tasks;

namespace Misaka.MessageQueue
{
    public interface IMessageBus
    {
        void Publish(object message);

        void Publish(IEnumerable messages);

        Task PublishAsync(object message);
        
        Task PublishAsync(IEnumerable messages);
    }
}
