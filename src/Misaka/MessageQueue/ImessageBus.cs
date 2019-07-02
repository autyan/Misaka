using System.Threading.Tasks;

namespace Misaka.MessageQueue
{
    public interface IMessageBus
    {
        void Publish(object message);

        Task PublishAsync(object message);
    }
}
