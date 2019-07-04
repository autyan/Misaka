using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Misaka.Domain;
using Misaka.Repository;

namespace Misaka.EntityFrameworkCore
{
    public class EfCoreRepository : IRepository
    {
        private readonly DbContext _dbContext;

        public EfCoreRepository(DbContext        dbContext, 
                                EfCoreUnitOfWork unitOfWork)
        {
            _dbContext  = dbContext;
            unitOfWork.RegisterDbContext(_dbContext);
        }

        private DbSet<TEntity> GetDbSet<TEntity>() 
            where TEntity: class, IEntity
            => _dbContext.Set<TEntity>();
        
        public TEntity FindByKey<TEntity>(params object[] keyValues) 
            where TEntity : class, IEntity
        {
            return GetDbSet<TEntity>().Find(keyValues);
        }

        public void Add<TEntity>(TEntity entity) 
            where TEntity : class, IEntity
        {
            GetDbSet<TEntity>().Add(entity);
        }

        public void Add<TEntity>(IEnumerable<TEntity> entities) 
            where TEntity : class, IEntity
        {
            GetDbSet<TEntity>().AddRange(entities);
        }

        public void Remove<TEntity>(TEntity entity) 
            where TEntity : class, IEntity
        {
            GetDbSet<TEntity>().Remove(entity);
        }

        public void Remove<TEntity>(IEnumerable<TEntity> entities) 
            where TEntity : class, IEntity
        {
            GetDbSet<TEntity>().RemoveRange(entities);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) 
            where TEntity : class, IEntity
        {
            return GetDbSet<TEntity>().Where(predicate);
        }
    }
}