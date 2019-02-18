using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUI.DataCollectorUIWpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DataCollectorUIWPF : UserControlBase
    {
        public DataCollectorUIWPF()
        {
            InitializeComponent();

            var viewModel = new DataCollectorViewModel();
            DataContext = viewModel;

        }
    }
}
