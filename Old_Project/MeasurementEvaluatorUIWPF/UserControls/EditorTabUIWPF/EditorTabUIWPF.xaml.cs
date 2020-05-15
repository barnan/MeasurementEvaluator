using Interfaces.Misc;
using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.EditorTabUIWPF
{
    /// <summary>
    /// Interaction logic for EditorTab.xaml
    /// </summary>
    public partial class EditorTabUIWPF : UserControlBase, ITabUIWPF
    {
        public EditorTabUIWPF(EditorTabUIWPFParameters parameters)
        {
            InitializeComponent();

            DataContext = new EditorTabViewModel(parameters);

            Name = parameters.Name;

            Title = "Editor Tab";
        }
    }
}
