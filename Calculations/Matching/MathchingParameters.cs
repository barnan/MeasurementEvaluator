using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.DataAcquisition;
using NLog;

namespace Calculations.Matching
{
    internal class MathchingParameters
    {
        internal ILogger Logger { get; private set; }

        [Configuration("Handles the reading of the matching file.", Name = "Name of Matching Component", LoadComponent = true)]
        private string _matchingFileReader = null;
        internal IHDDFileReader MatchingFileReader { get; private set; }

        [Configuration("Name of the matching file", Name = "Name of the matching file", LoadComponent = false)]
        private string _bindingFilePath = "MatchingDictionary";
        internal string BindingFilePath => _bindingFilePath;



        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);

            MatchingFileReader = PluginLoader.CreateInstance<IHDDFileReader>(_matchingFileReader);

            return CheckComponent();
        }


        private bool CheckComponent()
        {
            if (MatchingFileReader == null)
            {
                Logger.Error($"Error in the {nameof(MathchingParameters)} instantiation. {nameof(MatchingFileReader)} is null.");
                return false;
            }

            if (string.IsNullOrEmpty(BindingFilePath))
            {
                Logger.Error($"Error in the {nameof(MathchingParameters)} instantiation. {nameof(BindingFilePath)} is empty.");
                return false;
            }

            return true;
        }
    }
}
