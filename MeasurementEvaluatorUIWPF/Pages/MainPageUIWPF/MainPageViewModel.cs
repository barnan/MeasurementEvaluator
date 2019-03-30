using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.Pages.MainPageUIWPF
{
    internal class MainPageViewModel : ViewModelBase
    {
        private MainPageUIWPFParameters _parameters;
        internal MainPageUIWPFParameters Parameters => _parameters;

        internal MainPageViewModel(MainPageUIWPFParameters parameters)
        {
            _parameters = parameters;
        }


    }
}
