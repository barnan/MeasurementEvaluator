using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.Pages.EvaluationPage
{
    public class EvaluationPageViewModel : ViewModelBase
    {
        private EvaluationPageUIWPFParameters _parameters;
        public EvaluationPageUIWPFParameters Parameters => _parameters;



        public EvaluationPageViewModel(EvaluationPageUIWPFParameters parameters)
        {
            _parameters = parameters;
        }
    }
}
