using Interfaces.DataAcquisition;
using Miscellaneous;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAcquisition.Repository
{
    public abstract class HDDRepository<T> : IRepository<T>
        where T : class, IStoredDataOnHDD, IComparable<T>
    {
        protected readonly SimpleHDDRepositoryParameters _parameters;
        private readonly object _lockObject = new object();
        // TODO: use locking


        protected HDDRepository(SimpleHDDRepositoryParameters parameters)
        {
            _parameters = parameters;
        }

        #region IRepository<T>

        public virtual IEnumerable<T> Find(Predicate<T> predicate)
        {
            lock (_lockObject)
            {
                if (predicate == null)
                {
                    _parameters.Logger.MethodError("The arrived predicate is null.");
                    return null;
                }

                List<T> itemList = GetItemList(_parameters.RepositoryFullDirectoryPath);
                List<T> hitList = new List<T>();

                foreach (T item in itemList)
                {
                    if (predicate(item))
                    {
                        hitList.Add(item);
                    }
                }

                return hitList;
            }
        }


        public virtual T Get(int index, IComparer<T> comparer = null)
        {
            lock (_lockObject)
            {
                try
                {
                    if (index < 0)
                    {
                        _parameters.Logger.MethodError("The arrived index is below 0.");
                        return null;
                    }

                    List<T> itemList = GetItemList(_parameters.RepositoryFullDirectoryPath);

                    if (index > itemList.Count)
                    {
                        _parameters.Logger.MethodError("The arrived index is higher than the length of the specification list.");
                        return null;
                    }

                    if (comparer == null)
                    {
                        itemList.Sort();
                    }
                    else
                    {
                        itemList.Sort(comparer);
                    }

                    return itemList[index];

                }
                catch (Exception ex)
                {
                    _parameters.Logger.MethodError($"Exception occured: {ex}");
                    return null;
                }
            }
        }


        public virtual bool Add(T item)
        {
            lock (_lockObject)
            {
                try
                {
                    if (string.IsNullOrEmpty(item?.FullNameOnHDD))
                    {
                        _parameters.Logger.MethodError("Arrived specification is null or its filename is null or empty.");
                        return false;
                    }

                    if (File.Exists(item.FullNameOnHDD))
                    {
                        _parameters.Logger.MethodError($"The given file: {item.FullNameOnHDD} already exists.");
                        return false;
                    }

                    // TODO
                    //item.Save();

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.MethodTrace("Item added.");
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.MethodError($"Adding of {item} was not successful: {ex}");
                    return false;
                }
            }
        }


        public virtual void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }


        public virtual IEnumerable<T> GetAll()
        {
            lock (_lockObject)
            {
                return GetItemList(_parameters.RepositoryFullDirectoryPath);
            }
        }


        public virtual bool Remove(T item)
        {
            lock (_lockObject)
            {
                try
                {
                    if (string.IsNullOrEmpty(item?.FullNameOnHDD))
                    {
                        _parameters.Logger.MethodError("Arrived specification is null or its filename is null or empty.");
                        return false;
                    }

                    if (!File.Exists(item.FullNameOnHDD))
                    {
                        _parameters.Logger.MethodError($"The given file: {item.FullNameOnHDD} does not exist.");
                        return false;
                    }

                    File.Delete(item.FullNameOnHDD);

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.MethodTrace("Item removed.");
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.MethodError($"Remove of {item} was not successful: {ex}");
                    return false;
                }
            }
        }


        public virtual void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Remove(item);
            }
        }


        #endregion


        protected bool CheckFolder(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                _parameters.Logger.MethodError("The given path is null or empty.");
                return false;
            }

            if (!string.IsNullOrEmpty(Path.GetFileName(fullPath)))
            {
                _parameters.Logger.MethodError($"The given path ({fullPath}) contains filename too, it is not a folder.");
                return false;
            }

            if (!Directory.Exists(fullPath))
            {
                _parameters.Logger.MethodError($"The given folder does not exists: {fullPath}.");
                return false;
            }

            if (_parameters.Logger.IsTraceEnabled)
            {
                _parameters.Logger.MethodTrace($"The given directory path ({fullPath}) checked.");
            }

            return true;
        }


        protected abstract List<T> GetItemList(string fullPath);

    }


    public class SimpleHDDRepositoryParameters
    {
        public string RepositoryFullDirectoryPath { get; set; }
        public string FileExtensionFilter { get; set; }
        public ILogger Logger { get; set; }
        public IXmlReader XmlReader { get; set; }
        public IXmlParser XmlParser { get; set; }

    }


}
