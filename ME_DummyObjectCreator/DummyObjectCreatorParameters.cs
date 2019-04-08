using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.DataAcquisition;
using NLog;

namespace ME_DummyObjectCreator
{
    internal class DummyObjectCreatorParameters
    {

        public ILogger Logger { get; private set; }


        [Configuration("HDDFileReaderWriter", "HDDFileReaderWriter", LoadComponent = true)]
        private string _hddFileReaderWriter = null;

        public IHDDFileReaderWriter HDDFileReaderWriter { get; private set; }


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);

            HDDFileReaderWriter = PluginLoader.CreateInstance<IHDDFileReaderWriter>(_hddFileReaderWriter);

            return CheckComponents();
        }

        private bool CheckComponents()
        {
            if (HDDFileReaderWriter == null)
            {
                Logger.Error($"Error in the {nameof(DummyObjectCreatorParameters)} loading. {nameof(HDDFileReaderWriter)} is null.");
                return false;
            }

            return true;
        }


    }
}
