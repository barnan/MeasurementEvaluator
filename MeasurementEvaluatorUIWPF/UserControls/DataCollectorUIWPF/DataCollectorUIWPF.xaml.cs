﻿using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DataCollectorUIWPF : UserControlBase
    {
        public DataCollectorUIWPF(DataCollectorUIWPFParameters param)
        {
            InitializeComponent();

            var viewModel = new DataCollectorViewModel(param);
            DataContext = viewModel;

        }
    }


}