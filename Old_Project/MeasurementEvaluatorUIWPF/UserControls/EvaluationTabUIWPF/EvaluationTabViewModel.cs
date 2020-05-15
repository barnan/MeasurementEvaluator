using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.EvaluationTabUIWPF
{
    internal class EvaluationTabViewModel : ViewModelBase
    {
        public EvaluationTabUIWPFParameters Parameters { get; }



        public EvaluationTabViewModel(EvaluationTabUIWPFParameters parameters)
        {
            Parameters = parameters;
            Parameters.InitializationCompleted += Parameters_InitializationCompleted;
        }

        private void Parameters_InitializationCompleted(object sender, System.EventArgs e)
        {
            Parameters.InitializationCompleted -= Parameters_InitializationCompleted;
        }

    }
}
