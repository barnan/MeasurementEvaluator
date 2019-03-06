using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUIWPF.MessageControlUIWPF
{
    /// <summary>
    /// Interaction logic for MessageControlUIWPF.xaml
    /// </summary>
    public partial class MessageControlUIWPF : UserControlBase
    {
        public MessageControlUIWPF()
        {
            InitializeComponent();

            DataContext = new UIMessageControl();

        }
    }
}
