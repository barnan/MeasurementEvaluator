using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.DataAcquisition;
using NLog;

namespace MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF
{

    public class DataCollectorUIWPFParameters
    {

        [Configuration("Name of DataCollector component", "DataCollector Name", true)]
        private string _dataCollectorName = null;
        internal IDataCollector DataCollector { get; private set; }


        internal ILogger Logger { get; private set; }


        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            DataCollector = PluginLoader.CreateInstance<IDataCollector>(_dataCollectorName);

            return CheckComponent();
        }


        private bool CheckComponent()
        {
            if (DataCollector == null)
            {

            }

            return true;
        }
    }
}
