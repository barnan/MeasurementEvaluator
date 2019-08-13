using Interfaces.Result;
using System.Collections.Generic;
using System.Linq;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF
{
    public class DataGridElement
    {
        public string ToolName { get; set; }

        public string Name { get; set; }

        public string QuantityName { get; set; }

        public IConditionEvaluationResult ConditionEvaluationResult { get; set; }

        public double CalculationDoubleResult => (ConditionEvaluationResult.CalculationResult as ICalculationResult<double>)?.ResultValue ?? 0;

        public List<double> MeasurementPoints => ConditionEvaluationResult.CalculationResult.MeasurementSerie.MeasuredPoints.Select(p => p.Value).ToList();
    }


}
