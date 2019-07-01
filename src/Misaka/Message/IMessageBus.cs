using System.Threading.Tasks;

namespace Misaka.Message
{
    public interface IMessageBus
    {
        void Publish(object message);

        Task PublishAsync(object message);
    }
}
