﻿using Interfaces.DataAcquisition;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAcquisitions.Repository
{
    internal abstract class HDDRepository<T> : IRepository<T>
        where T : class, IStoredDataOnHDD, IComparable<T>
    {

        protected readonly HDDRepositoryParameters _parameters;
        private readonly object _lockObject = new object();



        protected HDDRepository(HDDRepositoryParameters parameters)
        {
            _parameters = parameters;
        }


        #region IInitializable

        public bool Initiailze()
        {
            if (IsInitialized)
            {
                return true;
            }

            lock (_lockObject)
            {
                if (IsInitialized)
                {
                    return true;
                }

                IsInitialized = true;

                OnInitialized();

                _parameters.Logger.MethodInfo("Initialized.");

                return IsInitialized;
            }
        }

        public void Close()
        {
            if (!IsInitialized)
            {
                return;
            }

            lock (_lockObject)
            {
                if (!IsInitialized)
                {
                    return;
                }

                IsInitialized = false;

                OnClosed();

                _parameters.Logger.MethodInfo("Closed.");
            }
        }

        public event EventHandler<EventArgs> Initialized;
        public event EventHandler<EventArgs> Closed;

        public bool IsInitialized { get; private set; }


        private void OnInitialized()
        {
            var initialized = Initialized;

            initialized?.Invoke(this, new EventArgs());
        }


        private void OnClosed()
        {
            var closed = Closed;

            closed?.Invoke(this, new EventArgs());
        }

        #endregion


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

                    _parameters.Logger.MethodInfo($"Element with index: {index} was given back.");

                    return itemList[index];

                }
                catch (Exception ex)
                {
                    _parameters.Logger.MethodError($"Exception occured: {ex}");
                    return null;
                }
            }
        }

        public virtual T Get(string name)
        {
            lock (_lockObject)
            {
                try
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        _parameters.Logger.MethodError("The arrived name is null or empty.");
                        return null;
                    }

                    List<T> itemList = GetItemList(_parameters.RepositoryFullDirectoryPath).Where(p => p.FullNameOnHDD == name).ToList();

                    if (itemList.Count == 0)
                    {
                        _parameters.Logger.MethodError($"No element was found with the given name: {name}.");
                        return null;
                    }

                    if (itemList.Count > 1)
                    {
                        _parameters.Logger.MethodError($"More elements were found with the given name: {name}.");
                        return null;
                    }

                    _parameters.Logger.MethodInfo($"Element with name: {name} was given back.");

                    return itemList[0];

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

                    // TODO : name with path arrives or not?
                    string fullName = Path.Combine(_parameters.RepositoryFullDirectoryPath, item.FullNameOnHDD);

                    if (File.Exists(item.FullNameOnHDD))
                    {
                        _parameters.Logger.MethodError($"The given file: {item.FullNameOnHDD} already exists.");
                        return false;
                    }

                    _parameters.HDDReaderWriter.WriteToFile(item, fullName);

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.MethodTrace($"Item added: {item.FullNameOnHDD}");
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


        public virtual IEnumerable<string> GetAllNames()
        {
            lock (_lockObject)
            {
                return GetItemList(_parameters.RepositoryFullDirectoryPath).Select(p => p.ToString()).ToList();
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
                        _parameters.Logger.MethodTrace($"Item removed: {item.FullNameOnHDD}");
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

}
