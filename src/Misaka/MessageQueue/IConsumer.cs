using System.Threading.Tasks;

namespace Misaka.MessageQueue
{
    public interface IConsumer
    {
        void Start(ConsumerOption option);

        Task StartAsync(ConsumerOption option);

        void Stop();
    }
}
