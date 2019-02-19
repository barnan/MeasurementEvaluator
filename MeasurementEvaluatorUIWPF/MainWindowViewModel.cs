using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MainWindowParameters _parameters;
        public MainWindowParameters Parameters => _parameters;




        public MainWindowViewModel(MainWindowParameters parameters)
        {
            _parameters = parameters;
        }
    }
}
