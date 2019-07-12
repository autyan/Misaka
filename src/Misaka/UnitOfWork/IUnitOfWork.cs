using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Misaka.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit(TransactionScopeOption scopeOption = TransactionScopeOption.Required,
                    IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task CommitAsync(TransactionScopeOption scopeOption    = TransactionScopeOption.Required,
                         IsolationLevel         isolationLevel = IsolationLevel.ReadCommitted);
    }
}
