using Frame.Configuration;
using Frame.PluginLoader;
using FrameInterfaces;

namespace BaseClasses.Repository
{
    public class GenericHDDRepositoryParameters
    {
        public IMyLogger Logger { get; set; }

        [Configuration("File extension of the files used in the given repository folder", "File Extensions", true)]
        private List<string> _fileExtensionFilters = new List<string> { "*.*" };
        public List<string> FileExtensionFilters => _fileExtensionFilters;

        [Configuration("StartupFolder", "StartupFolder")]
        private string _startupFolder = "";
        public string StartupFolder => _startupFolder;

        [Configuration("MaintenanceThreadCycleTime [ms]", "MaintenanceThreadCycleTime")]
        private int _maintenanceThreadCycleTime_ms = 1000;
        public int MaintenanceThreadCycleTime_ms => _maintenanceThreadCycleTime_ms;


        public bool Load(string sectionName)
        {
            Logger = PluginLoader.GetLogger(sectionName);

            PluginLoader.ConfigManager.Load(this, sectionName);

            return true;
        }

    }
}
