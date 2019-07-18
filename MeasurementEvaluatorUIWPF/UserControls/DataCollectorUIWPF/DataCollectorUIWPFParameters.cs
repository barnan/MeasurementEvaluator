using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.DataAcquisition;
using MeasurementEvaluatorUIWPF.Base;
using NLog;

namespace MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF
{
    public class DataCollectorUIWPFParameters : ParameterBase
    {
        [Configuration("Name of DataCollector component", "DataCollector", true)]
        private string _dataCollectorName = null;
        internal IDataCollector DataCollector { get; private set; }


        internal ILogger Logger { get; private set; }


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();
            Name = sectionName;

            PluginLoader.ConfigManager.Load(this, sectionName);
            DataCollector = PluginLoader.CreateInstance<IDataCollector>(_dataCollectorName);

            return CheckComponent();
        }


        private bool CheckComponent()
        {
            if (DataCollector == null)
            {
                return false;
            }

            return true;
        }
    }
}
