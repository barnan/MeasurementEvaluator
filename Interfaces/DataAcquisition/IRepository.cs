using Interfaces.Misc;
using System;
using System.Collections.Generic;

namespace Interfaces.DataAcquisition
{

    public interface IRepository : IInitializable
    {
        object Get(int index, IComparer<object> comparer = null);

        object Get(string name);

        IEnumerable<object> GetAllElements();

        IEnumerable<string> GetAllNames();

        IEnumerable<object> Find(Predicate<object> predicate);

        bool Add(object item);

        void AddRange(IEnumerable<object> items);

        bool Remove(object item);

        void RemoveRange(IEnumerable<object> items);

        bool SetFolder(string path);
    }

}
