using Interfaces.DataAcquisition;
using Miscellaneous;
using NLog;
using PluginLoading;

namespace Calculations.Matching
{
    internal class MathchingParameters
    {
        internal ILogger Logger { get; private set; }

        [Configuration("Handles the reading of the matching file.", Name = "Name of Matching Component", LoadComponent = true)]
        private string _matchingFileReader;
        internal IHDDFileReader MatchingFileReader { get; private set; }

        [Configuration("Name of the matching file", Name = "Name of the matching file", LoadComponent = false)]
        private string _bindingFilePath;
        internal string BindingFilePath => _bindingFilePath;

        internal bool Load()
        {
            Logger = LogManager.GetCurrentClassLogger();
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

            if (BindingFilePath == null)
            {
                Logger.Error($"Error in the {nameof(MathchingParameters)} instantiation. {nameof(BindingFilePath)} is null.");
                return false;
            }

            return true;
        }
    }
}
