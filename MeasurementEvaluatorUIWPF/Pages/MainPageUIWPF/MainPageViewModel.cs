using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.Pages.MainPageUIWPF
{
    public class MainPageViewModel : ViewModelBase
    {
        private MainPageUIWPFParameters _parameters;
        public MainPageUIWPFParameters Parameters => _parameters;

        internal MainPageViewModel(MainPageUIWPFParameters parameters)
        {
            _parameters = parameters;
        }


    }
}
