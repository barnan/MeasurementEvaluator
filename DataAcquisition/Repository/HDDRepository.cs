using Interfaces.DataAcquisition;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace DataAcquisition.Repository
{
    public abstract class HDDRepository<T> : IRepository<T> where T : class
    {
        protected readonly SimpleHDDRepositoryParameter _parameters;
        private readonly object _lockObject = new object();


        protected HDDRepository(SimpleHDDRepositoryParameter parameters)
        {
            _parameters = parameters;
        }

        #region IRepository<T>

        public abstract void Add(T item);

        public abstract void AddRange(IEnumerable<T> items);

        public abstract IEnumerable<T> Find(Expression<Func<T>> predicate);

        public abstract T Get(int index, IComparer<T> comparer = null);

        public virtual IEnumerable<T> GetAll()
        {
            return GetItemList(_parameters.FullDirectoryPath);
        }

        public abstract void Remove(T item);

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
                _parameters.Logger.Error("The given path is null or empty.");
                return false;
            }

            if (!string.IsNullOrEmpty(Path.GetFileName(fullPath)))
            {
                _parameters.Logger.Error($"The given path ({fullPath}) contains filename too, it is not a folder.");
                return false;
            }

            if (!Directory.Exists(fullPath))
            {
                _parameters.Logger.Error($"The given folder does not exists: {fullPath}.");
                return false;
            }

            return true;
        }


        protected abstract List<T> GetItemList(string fullPath);

    }


    public class SimpleHDDRepositoryParameter
    {
        public string FullDirectoryPath { get; set; }
        public string FileExtensionFilter { get; set; }
        public ILogger Logger { get; set; }
        public IXmlReader XmlReader { get; set; }
        public IXmlParser XmlParser { get; set; }

    }


}
