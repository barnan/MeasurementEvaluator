using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace DataAcquisition.Repository
    {
    abstract class SimpleHDDRepository<T> : IRepository<T> where T : class
        {
        protected SimpleHDDRepositoryParameter _parameters;

        //protected List<string> FileList { get; set; }


        protected SimpleHDDRepository(SimpleHDDRepositoryParameter parameters)
            {
            _parameters = parameters;
            //FileList = new List<string>();

            }

        public abstract void Add(T item);

        public abstract void AddRange(IEnumerable<T> items);

        public abstract IEnumerable<T> Find(Expression<Func<T>> predicate);

        public abstract T Get(int index, IComparer<T> comparer = null);

        public abstract IEnumerable<T> GetAll();

        public abstract void Remove(T item);

        public abstract void RemoveRange(IEnumerable<T> items);



        protected bool CheckFolder(string fullPath)
            {
            if (fullPath == null)
                {
                _parameters.Logger.Error("The given path is null.");
                return false;
                }

            if (!Directory.Exists(fullPath))
                {
                _parameters.Logger.Error($"The given folder does not exists: {fullPath}.");
                return false;
                }

            return true;
            }


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
