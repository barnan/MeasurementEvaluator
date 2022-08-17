using System.Windows.Threading;
using Interfaces.Misc;

namespace MeasurementEvaluatorUIWPF.Base
{
    public class ParameterBase : INamed
    {
        public string Name { get; internal set; }


        // to call the initialization completed for all ui components
        public event EventHandler InitializationCompleted;

        public void OnInitializationCompleted()
        {
            var eventhandlers = InitializationCompleted;
            eventhandlers?.Invoke(this, new EventArgs());
        }


        // to call the closed for all ui components
        public event EventHandler Closed;

        public void OnClosed()
        {
            var eventhandlers = Closed;
            eventhandlers?.Invoke(this, new EventArgs());
        }


        public Dispatcher MainWindowDispatcher;
    }
}
