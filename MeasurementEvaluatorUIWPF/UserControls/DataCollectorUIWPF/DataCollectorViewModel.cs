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

            AvailableReferenceFileList = Parameters.DataCollector.GetReferenceSamples();
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
        public List<ToolNames> AvailableToolList
        {
            get { return _availableToolList; }
            set
            {
                _availableToolList = value;
                OnPropertyChanged();
            }
        }

        private ToolNames _selectedToolName;
        public ToolNames SelectedToolName
        {
            get { return _selectedToolName; }
            set
            {
                if (_selectedToolName == value)
                {
                    return;
                }

                _selectedToolName = value;
                OnPropertyChanged();

                AvailableSpecificationList = Parameters.DataCollector.GetSpecifications(_selectedToolName);

            }
        }


        private List<IToolSpecification> _availableSpecificationList;
        public List<IToolSpecification> AvailableSpecificationList
        {
            get { return _availableSpecificationList; }
            set
            {
                _availableSpecificationList = value;
                OnPropertyChanged();
            }
        }

        private IToolSpecification _selectedSpecification;
        public IToolSpecification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
                OnPropertyChanged();
            }
        }

        private List<IReferenceSample> _availableReferenceFileList;
        public List<IReferenceSample> AvailableReferenceFileList
        {
            get { return _availableReferenceFileList; }
            set
            {
                _availableReferenceFileList = value;
                OnPropertyChanged();
            }
        }

        private IReferenceSample _selectedReferenece;
        public IReferenceSample SelectedReferenece
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
