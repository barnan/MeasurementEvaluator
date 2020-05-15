using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Evaluation;
using MeasurementEvaluatorUIWPF.Base;
using NLog;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF
{
    public class ResultGridUIWPFParameters : ParameterBase
    {

        [Configuration("Name of evaluator component", "Evaluator", true)]
        private string _evaluatorName = null;
        internal IEvaluation Evaluator { get; private set; }


        internal ILogger Logger { get; private set; }


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetLogger(sectionName);
            Name = sectionName;

            PluginLoader.ConfigManager.Load(this, sectionName);
            Evaluator = PluginLoader.CreateInstance<IEvaluation>(_evaluatorName);

            return CheckComponent();
        }


        private bool CheckComponent()
        {
            if (Evaluator == null)
            {
                return false;
            }

            return true;
        }
    }
}
