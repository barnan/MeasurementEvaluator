using Interfaces.Misc;
using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.EvaluationTab
{
    /// <summary>
    /// Interaction logic for EvaluationUserControlUIWPF.xaml
    /// </summary>
    public partial class EvaluationTabUIWPF : UserControlBase, IUserControlUIWPF
    {
        public EvaluationTabUIWPF(EvaluationTabUIWPFParameters parameters)
        {
            InitializeComponent();

            DataContext = new EvaluationTabViewModel(parameters);

            Name = parameters.ID;
        }
    }
}
