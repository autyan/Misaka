using System.Threading.Tasks;

namespace Misaka.MessageQueue
{
    public interface IProducer
    {
        void Publish(PublishContext context);

        Task PublishAsync(PublishContext context);

        string Name { get; }
    }
}
