using Interfaces.Misc;
using System.Windows.Controls;

namespace MeasurementEvaluatorUIWPF.Pages.EvaluationPage
{
    /// <summary>
    /// Interaction logic for EvaluationPageUIWPF.xaml
    /// </summary>
    public partial class EvaluationPageUIWPF : Page, IPageUIWPF
    {
        public EvaluationPageUIWPF(EvaluationPageUIWPFParameters parameters)
        {
            InitializeComponent();

            DataContext = new EvaluationPageViewModel(parameters);

            Title = parameters.ID;
        }
    }
}
