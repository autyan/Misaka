using System.Collections.Generic;
using System.Threading.Tasks;
using Misaka.MessageQueue;
using Misaka.UnitOfWork;

namespace Misaka.EntityFrameworkCore
{
    public class EventBus : IEventBus
    {
        private readonly IMessageBus _messageBus;

        private readonly List<IEvent> _eventQueue = new List<IEvent>();

        public EventBus(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void PrepareEvents(IEnumerable<IEvent> events)
        {
            _eventQueue.AddRange(events);
        }

        public async Task PublishEventsAsync()
        {
            foreach (var @event in _eventQueue)
            {
                await _messageBus.PublishAsync(@event);
            }
        }
    }
}