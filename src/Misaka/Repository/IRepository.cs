using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Misaka.Domain;

namespace Misaka.Repository
{
    public interface IRepository
    {
        TEntity FindByKey<TEntity>(params object[] keyValues) 
            where TEntity : class, IEntity;

        void Add<TEntity>(TEntity entity) 
            where TEntity : class, IEntity;

        void Add<TEntity>(IEnumerable<TEntity> entities) 
            where TEntity : class, IEntity;

        void Remove<TEntity>(TEntity entity) 
            where TEntity : class, IEntity;

        void Remove<TEntity>(IEnumerable<TEntity> entities) 
            where TEntity : class, IEntity;

        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) 
            where TEntity : class, IEntity;
    }
}
