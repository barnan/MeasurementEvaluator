using Frame.PluginLoader;
using MeasurementEvaluatorUIWPF.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.EditorTabUIWPF
{
    public class EditorTabUIWPFParameters : ParameterBase
    {

        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            Name = sectionName;

            return true;
        }
    }
}
