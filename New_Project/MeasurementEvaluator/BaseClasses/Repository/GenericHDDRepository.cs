using Interfaces.DataAcquisition;
using Interfaces.Misc;
using Utils;

namespace BaseClasses.Repository
{
    public abstract class GenericHDDRepository<T> : InitializableBase, IRepository<INamedContent<T>>
    {
        private readonly GenericHDDRepositoryParameters _parameters;
        private readonly object _lockObject = new object();
        private string _repositoryPath;
        private List<INamedContent<T>> _fileContentDictionaryCache;
        private Thread _maintenanceThread;
        private CancellationTokenSource _cts;
        private static string NAME_ATTRIBUTE_NAME = "Name";


        public GenericHDDRepository(GenericHDDRepositoryParameters parameters)
            : base(parameters.Logger)
        {
            _parameters = parameters;
        }

        #region InitializableBase

        protected override void InternalInit()
        {
            //check if the given startupfolder exists:
            if (!Directory.Exists(_parameters.StartupFolder))
            {
                throw new ArgumentException($"The given input parameter has useless value {_parameters.StartupFolder}", nameof(_parameters.StartupFolder));
            }

            _repositoryPath = _parameters.StartupFolder;

            _fileContentDictionaryCache = new List<INamedContent<T>>();

            _cts = new CancellationTokenSource();
            _maintenanceThread = new Thread(Maintenance)
            {
                Name = $"GenericHDDRepository Maintenance Thread",
                IsBackground = true
            };
            _maintenanceThread.Start(_cts.Token);
        }


        protected override void InternalClose()
        {
            _cts.Cancel();
            _maintenanceThread.Join();
        }

        #endregion

        #region IRepository

        public INamedContent<T> Get(int index, IComparer<INamedContent<T>> comparer = null)
        {
            lock (_lockObject)
            {
                try
                {
                    if (index < 0)
                    {
                        _parameters.Logger.Error("The arrived index is below 0.");
                        return null;
                    }

                    INamedContent<T> hit = GetHitByIndex(_fileContentDictionaryCache, index, comparer);

                    return hit;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.Error($"Exception occurred: {ex}");
                    return null;
                }
            }
        }

        public INamedContent<T> Get(string name)
        {
            lock (_lockObject)
            {
                try
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        _parameters.Logger.Error("Received name was null or empty");
                        return null;
                    }

                    List<INamedContent<T>> hitList = GetHitListByName(_fileContentDictionaryCache, name);

                    if (hitList.Count == 1)
                    {
                        return hitList[0];
                    }

                    _parameters.Logger.Error($"More elements were found with the given name: {name}");
                    return null;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.Error($"Exception occurred: {ex}");
                    return null;
                }
            }
        }

        public List<INamedContent<T>> GetAllElements()
        {
            lock (_lockObject)
            {
                return GetItemList(_repositoryPath).ToList();
            }
        }

        public List<INamedContent<T>> Find(Predicate<INamedContent<T>> predicate)
        {
            lock (_lockObject)
            {
                if (predicate != null)
                {
                    return GetHitListByPredicate(_fileContentDictionaryCache, predicate);
                }

                _parameters.Logger.Error("The received predicate is null.");
                return null;
            }
        }

        public bool Add(INamedContent<T> item)
        {
            lock (_lockObject)
            {
                try
                {
                    string fullName = Path.Combine(_repositoryPath, item.Name);

                    if (File.Exists(fullName))
                    {
                        _parameters.Logger.Error($"The created fileName: {fullName} already exists -> it will be overwritten!!");
                    }

                    WriteContentToFile(fullName, item.Content);
                }
                catch (Exception ex)
                {
                    _parameters.Logger.Error($"Adding of {item} was not successful: {ex}");
                    return false;
                }
            }

            return true;
        }



        public void AddRange(IEnumerable<INamedContent<T>> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public bool Remove(INamedContent<T> item)
        {
            lock (_lockObject)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        _parameters.Logger.Error("Arrived specification is null or its filename is null or empty.");
                        return false;
                    }


                    string fullName = Path.Combine(_repositoryPath, item.Name);
                    if (File.Exists(fullName))
                    {
                        _parameters.Logger.Error($"The given file: {item.Name} already exists.");
                        return false;
                    }

                    File.Delete(fullName);

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.Trace($"Item removed: {fullName}");
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.Error($"Remove of {item} was not successful: {ex}");
                    return false;
                }
            }
        }

        public void RemoveRange(IEnumerable<INamedContent<T>> items)
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

                if (!Directory.Exists(path))
                {
                    throw new ArgumentException($"The given input parameter has useless value {path}");
                }

                _repositoryPath = path;

                GetItemList(path);

                return true;
            }
        }

        #endregion

        #region private
        private List<INamedContent<T>> GetItemList(string fullPath)
        {
            List<INamedContent<T>> fileContentDictionary = new List<INamedContent<T>>();

            try
            {
                List<string> fileNameList = new List<string>();
                foreach (string filterItem in _parameters.FileExtensionFilters)
                {
                    fileNameList.AddRange(Directory.GetFiles(fullPath, $"*.{filterItem}"));
                }

                foreach (string rawFileName in fileNameList)
                {
                    try
                    {
                        INamedContent<T> content = CreateNamedObjectFromFile(rawFileName);
                        fileContentDictionary.Add(content);

                        _parameters.Logger.Trace($"File read: {rawFileName}, stored in dictionary as {content.Name}");
                    }
                    catch (Exception ex)
                    {
                        _parameters.Logger.Error($"Exception occurred during file read: {ex}");
                    }
                }

                _parameters.Logger.Trace($"The old {nameof(_fileContentDictionaryCache)} had {_fileContentDictionaryCache.Count} elements, the new list has {fileContentDictionary.Count} elements");

                _fileContentDictionaryCache = fileContentDictionary;
            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occurred: {ex}");
            }
            return fileContentDictionary;
        }

        private List<INamedContent<T>> GetHitListByPredicate(List<INamedContent<T>> itemList, Predicate<INamedContent<T>> predicate)
        {
            return itemList.Where(p => predicate(p)).ToList();
        }

        private List<INamedContent<T>> GetHitListByName(List<INamedContent<T>> itemList, string name)
        {
            return itemList.Where(p => p.Name == name).Select(x => x).ToList();
        }

        private INamedContent<T> GetHitByIndex(List<INamedContent<T>> itemList, int index, IComparer<INamedContent<T>> comparer = null)
        {
            if (index > itemList.Count)
            {
                _parameters.Logger.Error("The arrived index is higher than the length of the internal list.");
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

            _parameters.Logger.Info($"Element {itemList[index]} with index: {index} was given back.");

            return itemList[index];
        }

        private void Maintenance(object obj)
        {
            _parameters.Logger.Info($"{Thread.CurrentThread.Name} was started");

            CancellationToken token = (CancellationToken)obj;

            while (!token.IsCancellationRequested)
            {
                GetAllElements();

                token.WaitHandle.WaitOne(_parameters.MaintenanceThreadCycleTime_ms);
            }

            _parameters.Logger.Info($"{Thread.CurrentThread.Name} was finished");
        }

        #endregion

        #region protected

        protected abstract INamedContent<T> CreateNamedObjectFromFile(string fileName);

        protected abstract void WriteContentToFile(string fileName, T content);

        #endregion
    }
}
