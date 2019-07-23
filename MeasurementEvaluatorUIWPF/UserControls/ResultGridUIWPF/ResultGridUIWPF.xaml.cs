using Interfaces.Misc;
using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF
{
    public partial class ResultGridUIWPF : UserControlBase, IUserControlUIWPF
    {

        public ResultGridUIWPF(ResultGridUIWPFParameters parameters)
        {
            InitializeComponent();

            var viewModel = new ResultGridViewModel(parameters);
            DataContext = viewModel;
            Title = "Result Grid";

        }
    }
}
