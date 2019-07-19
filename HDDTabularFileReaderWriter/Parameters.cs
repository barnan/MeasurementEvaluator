using Frame.ConfigHandler;
using Frame.PluginLoader;
using NLog;

namespace DataAcquisitions.HDDTabularMeasurementFileReaderWriter
{
    internal class Parameters
    {
        [Configuration("Separator", "Separator", LoadComponent = false)]
        private char _separator = default(char);
        internal char Separator => _separator;


        [Configuration("Read Timeout [ms], the maximum file read time. Minimum time: 100 ms", "Read Timeout")]
        private int _fileReadTimeout = 1;
        internal int FileReadTimeout => _fileReadTimeout;


        public ILogger Logger { get; private set; }


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetLogger(sectionName);

            PluginLoader.ConfigManager.Load(this, sectionName);

            if (FileReadTimeout < 100)
            {
                Logger.Error($"Error in the {nameof(Parameters)} loading. {nameof(FileReadTimeout)} is smaller than its allowed minimum value (100 ms).");
                return false;
            }

            return true;
        }

    }
}
