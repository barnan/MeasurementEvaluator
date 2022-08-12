using Interfaces.Misc;

namespace Interfaces.DataAcquisition
{
    public interface IRepository<T> : IInitializable
    {
        T Get(int index, IComparer<T> comparer = null);

        T Get(string name);

        List<T> GetAllElements();

        List<T> Find(Predicate<T> predicate);

        bool Add(T item);

        void AddRange(IEnumerable<T> items);

        bool Remove(T item);

        void RemoveRange(IEnumerable<T> items);

        bool SetFolder(string path);
    }
}
