using System.Threading.Tasks;

namespace Misaka.MessageQueue
{
    public interface IConsumer
    {
        void Start();

        Task StartAsync();
    }
}
