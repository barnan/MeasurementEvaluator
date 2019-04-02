using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.DataAcquisition;
using NLog;
using System.Collections.Generic;

namespace DataAcquisitions.Repository
{
    internal class HDDRepositoryParameters
    {
        internal ILogger Logger { get; private set; }

        //[Configuration("Folder name of the repository.", "Repository folder", true)]
        //private string _repositoryDirectoryPath = "Path";
        //internal string RepositoryDirectoryPath { get; private set; }

        [Configuration("File extension of the files used in the given repository folder", "File Extension", true)]
        private List<string> _fileExtensionFilters = null;
        internal List<string> FileExtensionFilters => _fileExtensionFilters;

        [Configuration("Name of the reader writer component", "Name of the reader writer component", true)]
        private string _hddDReaderWriter = null;
        internal IHDDFileReaderWriter HDDReaderWriter { get; private set; }

        public bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);

            HDDReaderWriter = PluginLoader.CreateInstance<IHDDFileReaderWriter>(_hddDReaderWriter);

            return CheckComponent();
        }

        private bool CheckComponent()
        {
            if (HDDReaderWriter == null)
            {
                Logger.Error($"Error in the {nameof(HDDRepositoryParameters)} loading. {nameof(HDDReaderWriter)} is null.");
                return false;
            }

            if (FileExtensionFilters == null)
            {
                Logger.Error($"Error in the {nameof(HDDRepositoryParameters)} loading. {nameof(FileExtensionFilters)} is null.");
                return false;
            }

            //if (RepositoryDirectoryPath == null)
            //{
            //    Logger.Error($"Error in the {nameof(HDDRepositoryParameters)} loading. {nameof(RepositoryDirectoryPath)} is null.");
            //    return false;
            //}

            return true;
        }
    }
}