using Interfaces.DataAcquisition;
using Interfaces.Misc;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAcquisitions.ME_Repository
{
    internal class HDDRepository : IRepository
    {

        private readonly HDDRepositoryParameters _parameters;
        private readonly object _lockObject = new object();
        private string _repositoryPath;


        internal HDDRepository(HDDRepositoryParameters parameters)
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

                if (!Directory.Exists(_repositoryPath))
                {
                    _parameters.Logger.MethodError($"The given directory ({_repositoryPath}) does not exists.");
                    return IsInitialized = false;
                }

                bool oldInitState = IsInitialized;
                IsInitialized = true;
                OnInitStateChanged(IsInitialized, oldInitState);

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

                bool oldInitState = IsInitialized;
                IsInitialized = false;
                OnInitStateChanged(IsInitialized, oldInitState);

                _parameters.Logger.MethodInfo("Closed.");
            }
        }

        public event EventHandler<InitializationEventArgs> InitStateChanged;

        public bool IsInitialized { get; private set; }


        private void OnInitStateChanged(bool newState, bool oldState)
        {
            var initialized = InitStateChanged;
            initialized?.Invoke(this, new InitializationEventArgs(newState, oldState));
        }

        #endregion


        #region IRepository<object>

        public virtual IEnumerable<object> Find(Predicate<object> predicate)
        {
            lock (_lockObject)
            {
                if (predicate == null)
                {
                    _parameters.Logger.MethodError("The received predicate is null.");
                    return null;
                }

                IEnumerable<object> itemList = GetItemList(_repositoryPath).Select(P => P.Value);
                List<object> hitList = new List<object>();

                foreach (object item in itemList)
                {
                    if (predicate(item))
                    {
                        hitList.Add(item);
                    }
                }

                return hitList;
            }
        }


        public virtual object Get(int index, IComparer<object> comparer = null)
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

                    List<object> itemList = GetItemList(_repositoryPath).Select(p => p.Value).ToList();

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

        public virtual object Get(string name)
        {
            lock (_lockObject)
            {
                try
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        _parameters.Logger.MethodError($"Received name null or empty.");
                        return null;
                    }

                    List<KeyValuePair<string, object>> itemList = GetItemList(_repositoryPath).Where(p => p.Key == name).ToList();

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


        public virtual bool Add(object item)
        {
            lock (_lockObject)
            {
                try
                {
                    INamed named = item as INamed;

                    if (string.IsNullOrEmpty(named?.Name))
                    {
                        _parameters.Logger.MethodError($"Received object is not {nameof(INamed)} or its Name is null or empty.");
                        return false;
                    }

                    string fullName = Path.Combine(_repositoryPath, CreateFileNameFromName(named.Name));
                    if (File.Exists(fullName))
                    {
                        _parameters.Logger.MethodError($"The created fileName: {fullName} already exists.");
                        return false;
                    }

                    _parameters.HDDReaderWriter.WriteToFile(item, fullName);

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.MethodTrace($"Item added: {fullName}");
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


        public virtual void AddRange(IEnumerable<object> items)
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
                IEnumerable<KeyValuePair<string, object>> itemList = GetItemList( );
                List<string> nameList = new List<string>();
                foreach (KeyValuePair<string, object> pair in itemList)
                {
                    if (pair.Value is INamed namedPairValue)
                    {
                        nameList.Add(namedPairValue.Name);
                        continue;
                    }
                    nameList.Add(pair.Key);
                }
                return nameList;
            }
        }


        public virtual bool Remove(object item)
        {
            lock (_lockObject)
            {
                try
                {
                    INamed named = item as INamed;

                    if (string.IsNullOrEmpty(named?.Name))
                    {
                        _parameters.Logger.MethodError($"Received object is not {nameof(INamed)} or its Name is null or empty.");
                        return false;
                    }

                    List<KeyValuePair<string, object>> itemList = GetItemList(_repositoryPath);

                    var selectedItem = itemList.Where(p => p.Key == named.Name).ToList();
                    if (selectedItem.Count > 1)
                    {
                        _parameters.Logger.MethodError($"More files contain {named.Name} -> {string.Join(",", selectedItem.Select(p => p.Key))}");
                        return false;
                    }

                    if (selectedItem.Count < 1)
                    {
                        _parameters.Logger.MethodError($"No files contain {named.Name}");
                        return false;
                    }

                    File.Delete(selectedItem[0].Key);

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.MethodTrace($"Item removed: {selectedItem[0].Key}");
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


        public virtual void RemoveRange(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                Remove(item);
            }
        }


        public bool SetFolder(string path)
        {
            lock (_lockObject)
            {
                if (string.IsNullOrEmpty(path))
                {
                    _parameters.Logger.Error("The received path is null.");
                    return false;
                }

                _repositoryPath = path;

                if (!IsInitialized)
                {
                    return true;
                }

                Close();
                Initiailze();
            }

            return true;
        }

        #endregion


        private bool CheckFolder(string fullPath)
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

        private List<KeyValuePair<string, object>> GetItemList(string fullPath)
        {
            try
            {
                if (!CheckFolder(fullPath))
                {
                    _parameters.Logger.MethodError($"The given folder can not be used: {fullPath}");
                    return null;
                }

                List<string> fileNameList = new List<string>();
                foreach (string filterItem in _parameters.FileExtensionFilters)
                {
                    fileNameList.AddRange(Directory.GetFiles(fullPath, $"*.{filterItem}"));
                }

                List<KeyValuePair<string, object>> fileContentDictionary = new List<KeyValuePair<string, object>>(fileNameList.Count);

                foreach (string fileName in fileNameList)
                {

                    object obj = _parameters.HDDReaderWriter.ReadFromFile(fileName);

                    fileContentDictionary.Add(new KeyValuePair<string, object>(fileName, obj));

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.MethodTrace($"File read: {fileName}");
                    }
                }

                if (_parameters.Logger.IsTraceEnabled)
                {
                    foreach (var item in fileContentDictionary)
                    {
                        _parameters.Logger.MethodTrace($"Items: {item}");
                    }
                }

                return fileContentDictionary;
            }
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Exception occured: {ex}");
                return null;
            }
        }

        private string CreateFileNameFromName(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Received name is not appropriate.");
            }

            string[] nameElements = name.Split(' ');
            return nameElements[0];
        }

    }
}
