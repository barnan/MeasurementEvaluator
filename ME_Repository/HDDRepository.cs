﻿using Interfaces.DataAcquisition;
using Interfaces.Misc;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAcquisitions.ME_Repository
{
    internal class HDDRepository : InitializableBase, IRepository
    {

        private readonly HDDRepositoryParameters _parameters;
        private readonly object _lockObject = new object();
        private string _repositoryPath;
        private List<KeyValuePair<string, object>> _fileContentDictionaryCache;


        internal HDDRepository(HDDRepositoryParameters parameters)
            : base(parameters.Logger)
        {
            _parameters = parameters;
        }


        #region IInitializable

        protected override void InternalInit()
        {
            if (!CheckFolder(_repositoryPath))
            {
                _parameters.Logger.LogMethodError($"The given directory ({_repositoryPath}) does not exists.");
                InitializationState = InitializationStates.InitializationFailed;
            }

            InitializationState = InitializationStates.Initialized;

            _parameters.MessageControl.AddMessage($"{_parameters.Name} Repository initialized.");
        }

        protected override void InternalClose()
        {
            InitializationState = InitializationStates.NotInitialized;
        }

        #endregion

        #region IRepository<object>

        public IEnumerable<object> Find(Predicate<object> predicate)
        {
            lock (_lockObject)
            {
                if (predicate == null)
                {
                    _parameters.Logger.LogMethodError("The received predicate is null.");
                    return null;
                }

                List<object> hitList = GetHitListByPredicate(_fileContentDictionaryCache, predicate);

                if (hitList?.Count > 0)
                {
                    return hitList;
                }

                _fileContentDictionaryCache = GetItemList(_repositoryPath);
                hitList = GetHitListByPredicate(_fileContentDictionaryCache, predicate);
                return hitList;
            }
        }


        public object Get(int index, IComparer<object> comparer = null)
        {
            lock (_lockObject)
            {
                try
                {
                    if (index < 0)
                    {
                        _parameters.Logger.LogMethodError("The arrived index is below 0.");
                        return null;
                    }

                    object hit = GetHitByIndex(_fileContentDictionaryCache, index, comparer);

                    if (hit != null)
                    {
                        return hit;
                    }

                    _fileContentDictionaryCache = GetItemList(_repositoryPath);
                    hit = GetHitByIndex(_fileContentDictionaryCache, index, comparer);
                    return hit;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogMethodError($"Exception occured: {ex}");
                    return null;
                }
            }
        }


        public object Get(string name)
        {
            lock (_lockObject)
            {
                try
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        _parameters.Logger.LogMethodError("Received name null or empty.");
                        return null;
                    }

                    List<object> hitList = GetHitListByName(_fileContentDictionaryCache, name);

                    if (hitList?.Count == 1)
                    {
                        return hitList[0];
                    }

                    _fileContentDictionaryCache = GetItemList(_repositoryPath);
                    hitList = GetHitListByName(_fileContentDictionaryCache, name);

                    if (hitList.Count > 1)
                    {
                        _parameters.Logger.LogMethodError($"More elements were found with the given name: {name}.");
                        return null;
                    }

                    _parameters.Logger.LogMethodInfo($"Element with name: {name} was given back.");

                    return hitList[0];
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogMethodError($"Exception occured: {ex}");
                    return null;
                }
            }
        }


        public IEnumerable<object> GetAllElements()
        {
            lock (_lockObject)
            {
                try
                {
                    _fileContentDictionaryCache = GetItemList(_repositoryPath);
                    var valueList = _fileContentDictionaryCache.Select(x => x.Value);

                    return valueList;

                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogMethodError($"Exception occured: {ex}");
                    return null;
                }
            }
        }


        public bool Add(object item)
        {
            lock (_lockObject)
            {
                try
                {
                    if (!(item is INamed named))
                    {
                        _parameters.Logger.LogMethodError($"Received object is not {nameof(INamed)} or its Name is null or empty.");
                        return false;
                    }

                    string fullName = Path.Combine(_repositoryPath, CreateFileNameFromName(named.Name));
                    if (File.Exists(fullName))
                    {
                        _parameters.Logger.LogMethodError($"The created fileName: {fullName} already exists -> it will be overwritten!!");
                    }

                    _parameters.HDDReaderWriter.WriteToFile(item, fullName);

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.LogMethodTrace($"Item added: {fullName}");
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogMethodError($"Adding of {item} was not successful: {ex}");
                    return false;
                }
            }
        }


        public void AddRange(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }


        public IEnumerable<string> GetAllNames()
        {
            lock (_lockObject)
            {
                _fileContentDictionaryCache = GetItemList(_repositoryPath);
                List<string> nameList = new List<string>();

                foreach (KeyValuePair<string, object> pair in _fileContentDictionaryCache)
                {
                    nameList.Add((pair.Value as INamed)?.Name ?? pair.Key);
                }
                return nameList;
            }
        }


        public bool Remove(object item)
        {
            lock (_lockObject)
            {
                try
                {
                    INamed named = item as INamed;

                    if (string.IsNullOrEmpty(named?.Name))
                    {
                        _parameters.Logger.LogMethodError($"Received object is not {nameof(INamed)} or its Name is null or empty.");
                        return false;
                    }

                    Refresh();

                    var selectedItem = _fileContentDictionaryCache.Where(p => p.Key == named.Name).ToList();

                    if (selectedItem.Count < 1)
                    {
                        _parameters.Logger.LogMethodError($"No files contain {named.Name}");
                        return false;
                    }

                    if (selectedItem.Count > 1)
                    {
                        _parameters.Logger.LogMethodError($"More files contain {named.Name} -> {string.Join(",", selectedItem.Select(p => p.Key))}");
                    }

                    foreach (KeyValuePair<string, object> pair in selectedItem)
                    {
                        File.Delete(pair.Key);
                    }

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.LogMethodTrace($"Item removed: {selectedItem[0].Key}");
                    }

                    Refresh();

                    return true;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogMethodError($"Remove of {item} was not successful: {ex}");
                    return false;
                }
            }
        }


        public void RemoveRange(IEnumerable<object> items)
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

        #region private

        private void Refresh()
        {
            lock (_lockObject)
            {
                _fileContentDictionaryCache = GetItemList(_repositoryPath);
            }
        }

        private bool CheckFolder(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                _parameters.Logger.LogMethodError("The given path is null or empty.");
                return false;
            }

            if (!Directory.Exists(fullPath))
            {
                _parameters.Logger.LogMethodError($"The given path does not exists: {fullPath}.");
                return false;
            }

            if (_parameters.Logger.IsTraceEnabled)
            {
                _parameters.Logger.LogMethodTrace($"The given directory path ({fullPath}) checked.");
            }

            return true;
        }


        private List<KeyValuePair<string, object>> GetItemList(string fullPath)
        {
            try
            {
                List<string> fileNameList = new List<string>();
                foreach (string filterItem in _parameters.FileExtensionFilters)
                {
                    fileNameList.AddRange(Directory.GetFiles(fullPath, $"*.{filterItem}"));
                }

                List<KeyValuePair<string, object>> fileContentDictionary = new List<KeyValuePair<string, object>>(fileNameList.Count);

                foreach (string rawFileName in fileNameList)
                {
                    object obj = _parameters.HDDReaderWriter.ReadFromFile(rawFileName);

                    string nameInDictionary = rawFileName;
                    if (obj is INamed namedObject)
                    {
                        nameInDictionary = namedObject.Name;
                    }

                    fileContentDictionary.Add(new KeyValuePair<string, object>(nameInDictionary, obj));

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.LogMethodTrace($"File read: {rawFileName}, stored in dictionary as {nameInDictionary}");
                    }
                }

                _parameters.Logger.LogMethodTrace($"The old {nameof(_fileContentDictionaryCache)} had {_fileContentDictionaryCache?.Count} elements, the new list has {fileContentDictionary.Count} elements");

                _fileContentDictionaryCache = fileContentDictionary;

                return fileContentDictionary;
            }
            catch (Exception ex)
            {
                _parameters.Logger.LogMethodError($"Exception occured: {ex}");
                return null;
            }
        }


        private string CreateFileNameFromName(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Received name is not appropriate, it is null or empty.");
            }

            // todo: make longer name
            string[] nameElements = name.Split(' ');
            return nameElements[0];
        }


        private List<object> GetHitListByPredicate(List<KeyValuePair<string, object>> itemList, Predicate<object> predicate)
        {
            return itemList?.Where(p => predicate(p.Value)).Select(p => p.Value).ToList();
        }


        private List<object> GetHitListByName(List<KeyValuePair<string, object>> itemList, string name)
        {
            return itemList?.Where(p => p.Key == name).Select(x => x.Value).ToList();
        }


        private object GetHitByIndex(List<KeyValuePair<string, object>> itemList, int index, IComparer<object> comparer = null)
        {
            if (itemList == null)
            {
                return null;
            }

            if (index > itemList.Count)
            {
                _parameters.Logger.LogMethodError("The arrived index is higher than the length of the internal list.");
                return null;
            }

            List<object> valueList = itemList.Select(p => p.Value).ToList();

            if (comparer == null)
            {
                valueList.Sort();
            }
            else
            {
                valueList.Sort(comparer);
            }

            _parameters.Logger.LogMethodInfo($"Element {valueList[index]} with index: {index} was given back.");

            return valueList[index];
        }

        #endregion
    }
}
