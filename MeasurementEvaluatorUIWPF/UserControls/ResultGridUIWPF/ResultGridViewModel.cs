using Interfaces;
using Interfaces.Result;
using MeasurementEvaluatorUI.Base;
using System.Collections.Generic;
using System.Linq;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF
{
    internal class ResultGridViewModel : ViewModelBase
    {
        private ResultGridUIWPFParameters Parameters { get; }


        public ResultGridViewModel(ResultGridUIWPFParameters parameters)
        {
            Parameters = parameters;

            Parameters.Evaluator.SubscribeToResultReadyEvent(Evaluator_OnResultready);
        }



        #region Props

        private List<IConditionEvaluationResult> _conditionEvaluationResults;
        public List<IConditionEvaluationResult> ConditionEvaluationResults
        {
            get
            {
                return _conditionEvaluationResults;
            }
            set
            {
                _conditionEvaluationResults = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region private

        private void Evaluator_OnResultready(object sender, ResultEventArgs eventArgs)
        {
            if (!(eventArgs?.Result is IEvaluationResult evaluationResult))
            {
                ConditionEvaluationResults = null;
                return;
            }

            ConditionEvaluationResults = evaluationResult.QuantityEvaluationResults.SelectMany(p => p.ConditionEvaluationResults).ToList();
        }


        #endregion

    }
}
