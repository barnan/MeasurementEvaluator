using Interfaces.Misc;
using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.EvaluationTabUIWPF
{
    public partial class EvaluationTabUIWPF : UserControlBase, ITabUIWPF
    {
        public EvaluationTabUIWPF(EvaluationTabUIWPFParameters parameters)
        {
            InitializeComponent();

            DataContext = new EvaluationTabViewModel(parameters);

            Name = parameters.Name;
            Title = "Evaluation Tab";
        }
    }
}
