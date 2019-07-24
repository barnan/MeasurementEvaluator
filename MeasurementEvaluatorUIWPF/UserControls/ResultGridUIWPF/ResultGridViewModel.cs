using Interfaces;
using Interfaces.Result;
using MeasurementEvaluatorUI.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            ConditionEvaluationResults = new ObservableCollection<IConditionEvaluationResult>();
        }



        #region Props

        private ObservableCollection<IConditionEvaluationResult> _conditionEvaluationResults;
        public ObservableCollection<IConditionEvaluationResult> ConditionEvaluationResults
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

            var elements = evaluationResult.QuantityEvaluationResults.SelectMany(p => p.ConditionEvaluationResults);


            Action act = delegate ()
            {
                foreach (IConditionEvaluationResult element in elements)
                {
                    ConditionEvaluationResults.Add(element);
                }
            };

            _parameters.MainWindowDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, act);

        }

        #endregion

    }
}
