using System.Windows.Controls;

namespace MeasurementEvaluatorUIWPF.Pages.EvaluationPage2
{
    /// <summary>
    /// Interaction logic for EvaluationUserControlUIWPF.xaml
    /// </summary>
    public partial class EvaluationUserControlUIWPF : UserControl
    {
        public EvaluationUserControlUIWPF(EvaluationUserControlUIWPFParameters parameters)
        {
            InitializeComponent();

            DataContext = new EvaluationUserControlViewModel(parameters);

            Name = parameters.ID;
        }
    }
}
