using MeasurementEvaluatorUI.Base;
using MeasurementEvaluatorUIWPF.Pages.EvaluationPage;

namespace MeasurementEvaluatorUIWPF.Pages.EditorPageUIWPF
{
    public class EditorPageViewModel : ViewModelBase
    {
        private EvaluationPageUIWPFParameters _parameters;
        public EvaluationPageUIWPFParameters Parameters => _parameters;



        public EditorPageViewModel(EvaluationPageUIWPFParameters parameters)
        {
            _parameters = parameters;
        }
    }
}
