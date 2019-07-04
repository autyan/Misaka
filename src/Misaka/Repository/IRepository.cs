using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Misaka.Domain;

namespace Misaka.Repository
{
    public interface IRepository
    {
        TAggregateRoot FindByKey<TAggregateRoot>(params object[] keyValues) where TAggregateRoot : IAggregateRoot;

        void Add<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : IAggregateRoot;

        void Add<TAggregateRoot>(IEnumerable<TAggregateRoot> aggregateRoots) where TAggregateRoot : IAggregateRoot;

        void Remove<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : IAggregateRoot;

        void Remove<TAggregateRoot>(IEnumerable<TAggregateRoot> aggregateRoots) where TAggregateRoot : IAggregateRoot;

        IEnumerable<TAggregateRoot> Find<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> predicate) where TAggregateRoot : IAggregateRoot;
    }
}
