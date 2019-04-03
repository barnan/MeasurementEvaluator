using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Misc;

namespace MeasurementEvaluatorUIWPF.UserControls.EvaluationTabUIWPF
{
    public class EvaluationTabUIWPFParameters
    {
        [Configuration("Data Collector user control name", nameof(DataCollectorUIWPF), true)]
        private string _dataCollectorUIWPFName = null;
        public IUserControlUIWPF DataCollectorUIWPF { get; private set; }


        [Configuration("Result grid user control name", nameof(ResultGridUIWPF), true)]
        private string _resultGridUIWPFName = null;
        public IUserControlUIWPF ResultGridUIWPF { get; private set; }


        [Configuration("Result handling user control name", nameof(ResultHandlingUIWPF), true)]
        private string _resultHandlingUIWPFName = null;
        public IUserControlUIWPF ResultHandlingUIWPF { get; private set; }


        public string ID { get; private set; }



        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            DataCollectorUIWPF = PluginLoader.CreateInstance<IUserControlUIWPF>(_dataCollectorUIWPFName);
            ResultGridUIWPF = PluginLoader.CreateInstance<IUserControlUIWPF>(_resultGridUIWPFName);
            ResultGridUIWPF = PluginLoader.CreateInstance<IUserControlUIWPF>(_resultHandlingUIWPFName);

            ID = sectionName;

            return true;
        }
    }
}
