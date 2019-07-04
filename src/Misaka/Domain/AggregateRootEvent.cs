namespace Misaka.Domain
{
    public class AggregateRootEvent : IAggregateRootEvent
    {
        public AggregateRootEvent(object aggregateRootId)
        {
            AggregateRootId = aggregateRootId;
            Key             = aggregateRootId.ToString();
        }

        public object AggregateRootId { get; }

        public string Key { get; }
    }
}
