using System.Collections.Generic;
using System.Threading.Tasks;

namespace Misaka.UnitOfWork
{
    public interface IEventBus
    {
        void PrepareEvents(IEnumerable<IEvent> events);
        
        Task PublishEventsAsync();
    }
}