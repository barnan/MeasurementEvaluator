using Interfaces.BaseClasses;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using MeasurementEvaluatorUI.Base;
using MeasurementEvaluatorUI.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF
{
    internal class DataCollectorViewModel : ViewModelBase
    {
        #region fields

        private DataCollectorUIWPFParameters Parameters { get; }
        private IEnumerable<IToolSpecification> _allAvailableToolSpecifications;
        private IEnumerable<IReferenceSample> _allAvailableReferenceFiles;

        #endregion

        #region ctor

        public DataCollectorViewModel(DataCollectorUIWPFParameters parameters)
        {
            Parameters = parameters;
            Parameters.InitializationCompleted += Parameters_InitializationCompleted;
            Parameters.Closed += Parameters_Closed;

            BrowseMeasurementDataCommand = new RelayCommand(ExecuteBrowse, o => SelectedToolName != null && SelectedSpecification != null);
            CalculateCommand = new RelayCommand(ExecuteCalculate, o => SelectedToolName != null && SelectedSpecification != null && SelectedMeasurementFiles != null);
        }

        private void Parameters_Closed(object sender, EventArgs e)
        {
            Parameters.DataCollector.Close();
        }

        private void Parameters_InitializationCompleted(object sender, System.EventArgs e)
        {
            Parameters.InitializationCompleted -= Parameters_InitializationCompleted;

            _allAvailableToolSpecifications = Parameters.DataCollector.GetAvailableToolSpecifications();

            _allAvailableReferenceFiles = Parameters.DataCollector.GetReferenceSamples();

            foreach (IToolSpecification specification in _allAvailableToolSpecifications)
            {
                if (!AvailableToolList.Contains(specification.ToolName))
                {
                    AvailableToolList.Add(specification.ToolName);
                }
            }
        }

        #endregion

        #region commands

        private ICommand _calculateCommand;
        public ICommand CalculateCommand
        {
            get => _calculateCommand;
            set
            {
                _calculateCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _browseMeasurementDataCommand;
        public ICommand BrowseMeasurementDataCommand
        {
            get => _browseMeasurementDataCommand;
            set
            {
                _browseMeasurementDataCommand = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region properties

        #region tool

        private ObservableCollection<ToolNames> _availableToolList = new ObservableCollection<ToolNames>();
        public ObservableCollection<ToolNames> AvailableToolList
        {
            get => _availableToolList;
            set
            {
                _availableToolList = value;
                OnPropertyChanged();
            }
        }

        private ToolNames _selectedToolName;
        public ToolNames SelectedToolName
        {
            get => _selectedToolName;
            set
            {
                if (_selectedToolName == value)
                {
                    return;
                }

                _selectedToolName = value;
                OnPropertyChanged();

                AvailableSpecificationList = _allAvailableToolSpecifications.Where(p => p.ToolName == SelectedToolName);
            }
        }

        #endregion tool

        #region specification

        private IEnumerable<IToolSpecification> _availableSpecificationList;
        public IEnumerable<IToolSpecification> AvailableSpecificationList
        {
            get => _availableSpecificationList;
            set
            {
                _availableSpecificationList = value;
                SpecificationcomboBoxIsEnabled = SelectedToolName != null;
                OnPropertyChanged();
            }
        }

        private IToolSpecification _selectedSpecification;
        public IToolSpecification SelectedSpecification
        {
            get => _selectedSpecification;
            set
            {
                _selectedSpecification = value;

                if (_selectedSpecification != null)
                {
                    ((RelayCommand)BrowseMeasurementDataCommand).UpdateCanExecute();

                    AvailableReferenceFileList = _allAvailableReferenceFiles;
                    ReferencecomboBoxIsEnabled = AvailableReferenceFileList != null;
                }

                OnPropertyChanged();
            }
        }

        private bool _specificationcomboBoxIsEnabled;
        public bool SpecificationcomboBoxIsEnabled
        {
            get => _specificationcomboBoxIsEnabled;
            set
            {
                _specificationcomboBoxIsEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion specification

        #region referenece sample

        private IEnumerable<IReferenceSample> _availableReferenceFileList;
        public IEnumerable<IReferenceSample> AvailableReferenceFileList
        {
            get => _availableReferenceFileList;
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

        private bool _referencecomboBoxIsEnabled;
        public bool ReferencecomboBoxIsEnabled
        {
            get { return _referencecomboBoxIsEnabled; }
            set
            {
                _referencecomboBoxIsEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion referenece sample

        #region measurement data

        private IEnumerable<string> _selectedMeasurementFiles;
        public IEnumerable<string> SelectedMeasurementFiles
        {
            get { return _selectedMeasurementFiles; }
            set
            {
                _selectedMeasurementFiles = value;
                OnPropertyChanged();

                ((RelayCommand)CalculateCommand).UpdateCanExecute();
            }
        }

        #endregion

        #endregion

        #region private

        private void ExecuteCalculate()
        {
            Parameters.DataCollector.GatherData(SelectedSpecification, SelectedMeasurementFiles, SelectedReferenece);
        }

        private void ExecuteBrowse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = true, InitialDirectory = Parameters.DataCollector.MeasurementFolderPath };
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedMeasurementFiles = openFileDialog.FileNames.ToList();
            }
        }

        #endregion

    }
}
