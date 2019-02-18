using Interfaces.Misc;
using System.Windows.Controls;

namespace MeasurementEvaluatorUI.Base
{
    class UserControlBase : UserControl, IComponent
    {
        public string Title { get; protected set; }


    }
}
