using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAcquisition.Repository
{
    class SimpleHDDRepository<T> : IRepository<T> where T : class
    {
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Find(Expression<Func<T>> predicate)
        {
            throw new NotImplementedException();
        }

        public T Get(int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }
    }
}
