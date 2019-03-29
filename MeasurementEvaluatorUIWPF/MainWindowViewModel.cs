using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private MainWindowParameters _parameters;
        public MainWindowParameters Parameters => _parameters;




        internal MainWindowViewModel(MainWindowParameters parameters)
        {
            _parameters = parameters;
        }
    }
}
