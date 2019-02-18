using MeasurementEvaluatorUI.Base;
using System.Windows.Input;

namespace MeasurementEvaluatorUI.DataCollectorUIWPF
{
    internal class DataCollectorViewModel : ViewModelBase
    {
        #region fields

        private DataCollectorUIWPFParameters _parameters;

        #endregion


        #region commands

        private ICommand _calculateCommand;
        public ICommand CalculateCommand
        {
            get { return _calculateCommand; }
            set { _calculateCommand = value; }
        }


        private ICommand _browseMeasurementDataCommand;
        public ICommand BrowseMeasurementDataCommand
        {
            get { return _browseMeasurementDataCommand; }
            set { _browseMeasurementDataCommand = value; }
        }





        #endregion

    }
}
