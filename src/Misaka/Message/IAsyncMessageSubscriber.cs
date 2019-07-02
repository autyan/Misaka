using System.Threading.Tasks;

namespace Misaka.Message
{
    public interface IAsyncMessageSubscriber<in T>
    {
        Task HandleAsync(T message);
    }
}
