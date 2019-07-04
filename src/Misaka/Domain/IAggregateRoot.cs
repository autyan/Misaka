using System.Collections.Generic;

namespace Misaka.Domain
{
    public interface IAggregateRoot
    {
        void ClearEvents();

        IEnumerable<AggregateRootEvent> GetEvents();
    }
}
