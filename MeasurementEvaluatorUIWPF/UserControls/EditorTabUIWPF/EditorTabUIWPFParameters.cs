using Frame.PluginLoader;

namespace MeasurementEvaluatorUIWPF.UserControls.EditorTabUIWPF
{
    public class EditorTabUIWPFParameters
    {
        public string ID { get; private set; }



        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            ID = sectionName;

            return true;
        }
    }
}
