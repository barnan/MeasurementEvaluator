using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.EditorTabUIWPF
{
    internal class EditorTabViewModel : ViewModelBase
    {
        public EditorTabUIWPFParameters Parameters { get; }

        public EditorTabViewModel(EditorTabUIWPFParameters parameters)
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
