using System.Threading.Tasks;

namespace Misaka.Message
{
    public interface IMessageClient
    {
        void Publish(string topic, object message);

        Task PublishAsync(string topic, object message);
    }
}
