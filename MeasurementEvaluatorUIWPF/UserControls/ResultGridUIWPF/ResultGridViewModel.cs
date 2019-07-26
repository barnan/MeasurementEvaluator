using Interfaces.BaseClasses;
using Interfaces.Result;
using MeasurementEvaluatorUI.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF
{
    internal class ResultGridViewModel : ViewModelBase
    {
        private ResultGridUIWPFParameters _parameters;


        public ResultGridViewModel(ResultGridUIWPFParameters parameters)
        {
            _parameters = parameters;
            _parameters.Evaluator.SubscribeToResultReadyEvent(Evaluator_OnResultReady);
            ConditionEvaluationResults = new ObservableCollection<DataGridElement>();
        }


        #region Props

        private IEvaluationResult _evaluationResult;
        public IEvaluationResult EvaluationResult
        {
            get
            {
                return _evaluationResult;
            }
            set
            {
                _evaluationResult = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<DataGridElement> _conditionEvaluationResults;
        public ObservableCollection<DataGridElement> ConditionEvaluationResults
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

        private void Evaluator_OnResultReady(object sender, ResultEventArgs eventArgs)
        {
            if (!(eventArgs?.Result is IEvaluationResult evaluationResult))
            {
                ConditionEvaluationResults = null;
                return;
            }

            EvaluationResult = evaluationResult;

            Action act = delegate ()
            {
                ConditionEvaluationResults.Clear();
                foreach (IQuantityEvaluationResult quantityEvaluatinResult in evaluationResult.QuantityEvaluationResults)
                {
                    foreach (IConditionEvaluationResult conditionEvaluationResult in quantityEvaluatinResult.ConditionEvaluationResults)
                    {
                        ConditionEvaluationResults.Add(new DataGridElement
                        {
                            Name = evaluationResult.Name,
                            ToolName = evaluationResult.ToolName.Name,
                            QuantityName = quantityEvaluatinResult.Quantity.Name,
                            ConditionEvaluationResult = conditionEvaluationResult
                        });
                    }
                }
            };

            _parameters.MainWindowDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, act);
        }

        #endregion

    }
}
