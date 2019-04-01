using Interfaces.Misc;
using System.Windows.Controls;

namespace MeasurementEvaluatorUIWPF.Pages.EditorPageUIWPF
{
    /// <summary>
    /// Interaction logic for EditorPageUIWPF.xaml
    /// </summary>
    public partial class EditorPageUIWPF : Page, IPageUIWPF
    {
        private EditorPageUIWPFParameters _param; 

        public EditorPageUIWPF(EditorPageUIWPFParameters param)
        {
            _param = param;

            InitializeComponent();
        }
    }
}
