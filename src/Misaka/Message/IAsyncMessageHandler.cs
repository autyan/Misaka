using System.Threading.Tasks;

namespace Misaka.Message
{
    public interface IAsyncMessageHandler
    {
        Task HandleAsync<T>(T message);
    }
}
