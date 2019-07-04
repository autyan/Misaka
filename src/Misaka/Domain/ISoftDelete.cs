using System;

namespace Misaka.Domain
{
    public interface ISoftDelete
    {
        DateTime DeletedAt { get; }

        bool Deleted { get; }
    }
}
