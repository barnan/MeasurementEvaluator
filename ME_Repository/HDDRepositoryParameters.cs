using Frame.ConfigHandler;
using Frame.MessageHandler;
using Frame.PluginLoader;
using Interfaces.DataAcquisition;
using NLog;
using System.Collections.Generic;
using System.Linq;

namespace DataAcquisitions.ME_Repository
{
    internal class HDDRepositoryParameters
    {
        internal ILogger Logger { get; private set; }


        [Configuration("File extension of the files used in the given repository folder", "File Extensions", true)]
        private List<string> _fileExtensionFilters = new List<string> { "*.*" };
        internal List<string> FileExtensionFilters => _fileExtensionFilters;


        [Configuration("Name of the reader writer component", "Reader writer", true)]
        private string _hddDReaderWriter = null;
        internal IHDDFileReaderWriter HDDReaderWriter { get; private set; }


        public IUIMessageControl MessageControl { get; private set; }


        public string Name { get; private set; }


        public bool Load(string sectionName)
        {
            Name = sectionName;

            Logger = LogManager.GetLogger(sectionName);

            PluginLoader.ConfigManager.Load(this, sectionName);

            HDDReaderWriter = PluginLoader.CreateInstance<IHDDFileReaderWriter>(_hddDReaderWriter);

            MessageControl = PluginLoader.MessageControll;

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

            if (FileExtensionFilters.Any(p => string.IsNullOrEmpty(p)))
            {
                Logger.Error($"Error in the {nameof(HDDRepositoryParameters)} loading. {nameof(FileExtensionFilters)} has empty element.");
                return false;
            }

            if (MessageControl == null)
            {
                Logger.Error($"Error in the {nameof(HDDRepositoryParameters)} loading. {nameof(MessageControl)} is null.");
                return false;
            }

            return true;
        }
    }
}