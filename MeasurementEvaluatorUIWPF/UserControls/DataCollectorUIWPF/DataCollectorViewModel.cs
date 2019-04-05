using MeasurementEvaluatorUI.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF
{
    internal class DataCollectorViewModel : ViewModelBase
    {
        #region fields

        private DataCollectorUIWPFParameters _parameters;

        #endregion

        #region ctor

        public DataCollectorViewModel(DataCollectorUIWPFParameters parameters)
        {
            _parameters = parameters;
        }

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


        #region properties

        private ObservableCollection<string> _availableToolList;
        private ObservableCollection<string> AvailableToolList
        {
            get { return _availableToolList; }
            set
            {
                _availableToolList = value;
                OnPropertyChanged();
            }
        }

        private string _selectedToolName;
        private string SelectedToolName
        {
            get { return _selectedToolName; }
            set
            {
                _selectedToolName = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<string> _availableSpecificationList;
        private ObservableCollection<string> AvailableSpecificationList
        {
            get { return _availableSpecificationList; }
            set
            {
                _availableSpecificationList = value;
                OnPropertyChanged();
            }
        }

        private string _selectedSpecificationName;
        private string SelectedSpecificationName
        {
            get { return _selectedSpecificationName; }
            set
            {
                _selectedSpecificationName = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _availableReferenceFileList;
        private ObservableCollection<string> AvailableReferenceFileList
        {
            get { return _availableReferenceFileList; }
            set
            {
                _availableReferenceFileList = value;
                OnPropertyChanged();
            }
        }

        private string _selectedRefereneceName;
        private string SelectedRefereneceName
        {
            get { return _selectedRefereneceName; }
            set
            {
                _selectedRefereneceName = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
