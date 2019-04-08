using Frame.ConfigHandler;
using Frame.PluginLoader;
using NLog;

namespace DataAcquisitions.HDDTabularMeasurementFileReaderWriter
{
    internal class Parameters
    {
        [Configuration("Separator", "Separator", LoadComponent = false)]
        private char _separator;
        internal char Separator => _separator;

        public ILogger Logger { get; private set; }


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);

            return true;
        }

    }
}
