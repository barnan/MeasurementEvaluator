using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAcquisition.Repository
{
    abstract class SimpleHDDRepository<T> : IRepository<T> where T : class
    {
        protected string Path { get; set; }
        protected string ExtensionFilter { get; set; }


        public abstract void Add(T item);

        public abstract void AddRange(IEnumerable<T> items);

        public abstract IEnumerable<T> Find(Expression<Func<T>> predicate);

        public abstract T Get(int index);

        public abstract IEnumerable<T> GetAll();

        public abstract void Remove(T item);

        public abstract void RemoveRange(IEnumerable<T> items);
    }
}
