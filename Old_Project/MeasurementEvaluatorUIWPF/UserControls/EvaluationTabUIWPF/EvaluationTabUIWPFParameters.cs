using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Misc;
using MeasurementEvaluatorUIWPF.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.EvaluationTabUIWPF
{
    public class EvaluationTabUIWPFParameters : ParameterBase
    {
        [Configuration("Data Collector user control name", nameof(DataCollectorUIWPF), true)]
        private string _dataCollectorUIWPFName = null;
        public IUserControlUIWPF DataCollectorUIWPF { get; private set; }


        [Configuration("Result grid user control name", nameof(ResultGridUIWPF), true)]
        private string _resultGridUIWPFName = null;
        public IUserControlUIWPF ResultGridUIWPF { get; private set; }


        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            DataCollectorUIWPF = PluginLoader.CreateInstance<IUserControlUIWPF>(_dataCollectorUIWPFName);
            ResultGridUIWPF = PluginLoader.CreateInstance<IUserControlUIWPF>(_resultGridUIWPFName);

            Name = sectionName;

            return true;
        }
    }
}
