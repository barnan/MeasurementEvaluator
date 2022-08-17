using System.Windows.Controls;
using Interfaces.Misc;

namespace MeasurementEvaluatorUIWPF.Base
{
    public class UserControlBase : UserControl, INamed
    {

        public EventHandler<EventArgs> InitializationFinished;

    }
}
