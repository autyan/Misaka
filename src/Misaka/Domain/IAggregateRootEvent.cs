using Misaka.UnitOfWork;

namespace Misaka.Domain
{
    public interface IAggregateRootEvent : IEvent
    {
        string Key { get; }
    }
}
