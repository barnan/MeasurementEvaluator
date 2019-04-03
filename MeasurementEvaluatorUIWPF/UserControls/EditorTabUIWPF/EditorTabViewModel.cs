using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.EditorTabUIWPF
{
    class EditorTabViewModel : ViewModelBase
    {
        public EditorTabUIWPFParameters Parameters { get; }

        public EditorTabViewModel(EditorTabUIWPFParameters parameters)
        {
            Parameters = parameters;
        }
    }
}
