using Interfaces.Misc;
using System;
using System.Collections.Generic;

namespace Interfaces.DataAcquisition
{

    public interface IRepository<T> : IInitializable
         where T : class, IComparable<T>, INamed
    {

        T Get(int index, IComparer<T> comparer = null);

        T Get(string name);

        IEnumerable<string> GetAllNames();

        IEnumerable<T> Find(Predicate<T> predicate);

        bool Add(T item);

        void AddRange(IEnumerable<T> items);

        bool Remove(T item);

        void RemoveRange(IEnumerable<T> items);
    }

}
