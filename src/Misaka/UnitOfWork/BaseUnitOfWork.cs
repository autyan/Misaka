using System.Threading.Tasks;
using System.Transactions;

namespace Misaka.UnitOfWork
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        protected bool InTransaction => Transaction.Current != null;
        
        public virtual void Commit()
        {
            DoCommitAsync().Wait();
        }

        public virtual async Task CommitAsync()
        {
            await DoCommitAsync();
        }

        protected abstract Task DoCommitAsync();

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