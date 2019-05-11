using Interfaces;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using MeasurementEvaluatorUI.Base;
using System.Collections.Generic;
using System.Windows.Input;

namespace MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF
{
    internal class DataCollectorViewModel : ViewModelBase
    {
        #region fields

        private DataCollectorUIWPFParameters Parameters { get; }

        #endregion

        #region ctor

        public DataCollectorViewModel(DataCollectorUIWPFParameters parameters)
        {
            Parameters = parameters;
            Parameters.InitializationCompleted += Parameters_InitializationCompleted;
        }

        private void Parameters_InitializationCompleted(object sender, System.EventArgs e)
        {
            Parameters.InitializationCompleted -= Parameters_InitializationCompleted;

            AvailableToolList = Parameters.DataCollector.GetAvailableToolNames();
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

        private List<ToolNames> _availableToolList;
        private List<ToolNames> AvailableToolList
        {
            get { return _availableToolList; }
            set
            {
                _availableToolList = value;
                OnPropertyChanged();
            }
        }

        private ToolNames _selectedToolName;
        private ToolNames SelectedToolName
        {
            get { return _selectedToolName; }
            set
            {
                _selectedToolName = value;
                OnPropertyChanged();
            }
        }


        private List<IToolSpecification> _availableSpecificationList;
        private List<IToolSpecification> AvailableSpecificationList
        {
            get { return _availableSpecificationList; }
            set
            {
                _availableSpecificationList = value;
                OnPropertyChanged();
            }
        }

        private IToolSpecification _selectedSpecification;
        private IToolSpecification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
                OnPropertyChanged();
            }
        }

        private List<IReferenceSample> _availableReferenceFileList;
        private List<IReferenceSample> AvailableReferenceFileList
        {
            get { return _availableReferenceFileList; }
            set
            {
                _availableReferenceFileList = value;
                OnPropertyChanged();
            }
        }

        private IReferenceSample _selectedReferenece;
        private IReferenceSample SelectedReferenece
        {
            get { return _selectedReferenece; }
            set
            {
                _selectedReferenece = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
