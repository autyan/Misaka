using System.Threading.Tasks;
using System.Transactions;

namespace Misaka.UnitOfWork
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        protected bool InTransaction => Transaction.Current != null;
        
        public virtual void Commit(TransactionScopeOption scopeOption    = TransactionScopeOption.Required,
                                   IsolationLevel         isolationLevel = IsolationLevel.ReadCommitted)
        {
            DoCommitAsync(scopeOption, isolationLevel).Wait();
        }

        public virtual async Task CommitAsync(TransactionScopeOption scopeOption    = TransactionScopeOption.Required,
                                              IsolationLevel         isolationLevel = IsolationLevel.ReadCommitted)
        {
            await DoCommitAsync(scopeOption, isolationLevel);
        }

        protected abstract Task DoCommitAsync(TransactionScopeOption scopeOption,
                                              IsolationLevel         isolationLevel);

        protected virtual Task BeforeCommitAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task AfterCommitAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
        }
    }
}