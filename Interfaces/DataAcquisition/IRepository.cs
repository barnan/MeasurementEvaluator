using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Interfaces.DataAcquisition
{
    public interface IRepository<T> where T : class
    {
        T Get(int index, IComparer<T> comparer = null);

        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Expression<Func<T>> predicate);

        bool Add(T item);

        void AddRange(IEnumerable<T> items);

        bool Remove(T item);

        void RemoveRange(IEnumerable<T> items);

    }
}
