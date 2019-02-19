using MeasurementEvaluatorUI.Base;
using MeasurementEvaluatorUI.DataCollectorUIWPF;

namespace MeasurementEvaluatorUIWPF.DataCollectorUIWPF
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
