using Interfaces.Result;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF
{
    public class DataGridElement
    {
        public string ToolName { get; set; }

        public string Name { get; set; }

        public string QuantityName { get; set; }

        public IConditionEvaluationResult ConditionEvaluationResult { get; set; }
    }
}
