using Misaka.MessageQueue;
using System.Threading.Tasks;

namespace Misaka.MessageStore
{
    public interface IMessageStore
    {
        Task SavePublishAsync(PublishContext context);

        Task SaveConsumeAsync(MessageHandleContext contexts);
    }
}
