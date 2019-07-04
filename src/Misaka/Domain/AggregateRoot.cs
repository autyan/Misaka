using System.Collections.Generic;

namespace Misaka.Domain
{
    public class AggregateRoot : Entity, IAggregateRoot
    {
        private Queue<AggregateRootEvent> EventQueue { get; }= new Queue<AggregateRootEvent>();

        protected void OnEvent(AggregateRootEvent @event)
        {
            EventQueue.Enqueue(@event);
        }

        public virtual void ClearEvents()
        {
            EventQueue.Clear();
        }

        public virtual IEnumerable<AggregateRootEvent> GetEvents()
        {
            return EventQueue.ToArray();
        }
    }
}
