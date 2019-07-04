using System;
using System.Threading.Tasks;

namespace Misaka.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        Task CommitAsync();
    }
}
