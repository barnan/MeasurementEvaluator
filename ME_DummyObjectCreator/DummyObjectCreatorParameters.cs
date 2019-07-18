using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.DataAcquisition;
using NLog;

namespace ME_DummyObjectCreator
{
    internal class DummyObjectCreatorParameters
    {

        public ILogger Logger { get; private set; }


        [Configuration("HDDFileReaderWriter1", "HDDFileReaderWriter1", LoadComponent = true)]
        private string _hddFileReaderWriter1 = null;
        public IHDDFileReaderWriter HDDFileReaderWriter1 { get; private set; }

        [Configuration("HDDFileReaderWriter2", "HDDFileReaderWriter2", LoadComponent = true)]
        private string _hddFileReaderWriter2 = null;
        public IHDDFileReaderWriter HDDFileReaderWriter2 { get; private set; }


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);

            HDDFileReaderWriter1 = PluginLoader.CreateInstance<IHDDFileReaderWriter>(_hddFileReaderWriter1);
            HDDFileReaderWriter2 = PluginLoader.CreateInstance<IHDDFileReaderWriter>(_hddFileReaderWriter2);

            return CheckComponents();
        }

        private bool CheckComponents()
        {
            if (HDDFileReaderWriter1 == null)
            {
                Logger.Error($"Error in the {nameof(DummyObjectCreatorParameters)} loading. {nameof(HDDFileReaderWriter1)} is null.");
                return false;
            }

            if (HDDFileReaderWriter2 == null)
            {
                Logger.Error($"Error in the {nameof(DummyObjectCreatorParameters)} loading. {nameof(HDDFileReaderWriter2)} is null.");
                return false;
            }

            return true;
        }


    }
}
