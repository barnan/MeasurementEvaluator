using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Misc;

namespace MeasurementEvaluatorUIWPF.Pages.EvaluationPage
{
    public class EvaluationPageUIWPFParameters
    {

        [Configuration("Data Collector user control name", nameof(DataCollectorUIWPF), true)]
        private string _dataCollectorUIWPF;
        public IUserControlUIWPF DataCollectorUIWPF { get; private set; }


        [Configuration("Result grid user control name", nameof(ResultGridUIWPF), true)]
        private string _resultGridUIWPF;
        public IUserControlUIWPF ResultGridUIWPF { get; private set; }


        [Configuration("Result handling user control name", nameof(ResultHandlingUIWPF), true)]
        private string _resultHandlingUIWPF;
        public IUserControlUIWPF ResultHandlingUIWPF { get; private set; }


        public string ID { get; }



        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            DataCollectorUIWPF = PluginLoader.CreateInstance<IUserControlUIWPF>(_dataCollectorUIWPF);
            ResultGridUIWPF = PluginLoader.CreateInstance<IUserControlUIWPF>(_resultGridUIWPF);
            ResultGridUIWPF = PluginLoader.CreateInstance<IUserControlUIWPF>(_resultHandlingUIWPF);

            return true;
        }

    }
}
