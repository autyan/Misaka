using System.Collections.Generic;

namespace Misaka.Domain
{
    public class AggregateRoot : Entity, IAggregateRoot
    {
        public Queue<AggregateRootEvent> EventQueue { get; }= new Queue<AggregateRootEvent>();

        protected void OnEvent(AggregateRootEvent @event)
        {
            EventQueue.Enqueue(@event);
        }
    }
}
