using Interfaces.Misc;
using System.Windows.Controls;

namespace MeasurementEvaluatorUI.Base
{
    public class UserControlBase : UserControl, IComponent
    {
        public string Title { get; protected set; }


    }
}
