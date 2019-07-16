using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Misaka.Domain;
using Misaka.UnitOfWork;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Misaka.EntityFrameworkCore
{
    public class EfCoreUnitOfWork : BaseUnitOfWork
    {
        private readonly List<DbContext> _dbContexts = new List<DbContext>();

        private readonly IEventBus _eventBus;

        public EfCoreUnitOfWork(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        internal void RegisterDbContext(DbContext dbContext)
        {
            if (_dbContexts.Contains(dbContext)) return;
            _dbContexts.Add(dbContext);
        }
        
        protected override async Task DoCommitAsync(TransactionScopeOption scopeOption,
                                                    IsolationLevel         isolationLevel)
        {
            try
            {
                async Task CommitAction()
                {
                    _dbContexts.ForEach(async context =>
                                        {
                                            await context.SaveChangesAsync();
                                            foreach (var entityEntry in context.ChangeTracker.Entries())
                                            {
                                                if (!(entityEntry is IAggregateRoot aggregateRoot)) continue;
                                                
                                                _eventBus.PrepareEvents(aggregateRoot.GetEvents());
                                                aggregateRoot.ClearEvents();
                                            }
                                        });
                
                    await BeforeCommitAsync();
                }
                if (InTransaction)
                {
                    await CommitAction();
                }
                else
                {
                    using (var scope = new TransactionScope(scopeOption,
                                                            new TransactionOptions { IsolationLevel = isolationLevel },
                                                            TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await CommitAction();
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateConcurrencyException)
                {
                    throw new DBConcurrencyException(ex.Message, ex);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                await AfterCommitAsync();
            }
        }

        protected override async Task BeforeCommitAsync()
        {
            await _eventBus.PublishEventsAsync();
            await base.BeforeCommitAsync();
        }

        public override void Dispose()
        {
            foreach (var dbContext in _dbContexts)
            {
                dbContext.Dispose();
            }
            base.Dispose();
        }
    }
}