using System;

namespace Misaka.Domain
{
    public interface ITrace
    {
        DateTime CreateAt { get; }

        DateTime ModifyAt { get; }
    }
}
