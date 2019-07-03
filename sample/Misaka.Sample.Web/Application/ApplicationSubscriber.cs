using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Misaka.Message;

namespace Misaka.Sample.Web.Application
{
    public class ApplicationSubscriber : IAsyncMessageSubscriber<ValueEvent>
    {
        private readonly IMemoryCache _cache;

        public ApplicationSubscriber(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task HandleAsync(ValueEvent message)
        {
            _cache.CreateEntry(message.Id).Value = message.Value;
            return Task.CompletedTask;
        }
    }
}
