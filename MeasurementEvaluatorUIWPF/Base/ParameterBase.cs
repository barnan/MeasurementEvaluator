using Interfaces.Misc;
using System;

namespace MeasurementEvaluatorUIWPF.Base
{
    public class ParameterBase : INamed
    {
        public string Name { get; internal set; }

        private static event EventHandler _initializationCompleted;
        public event EventHandler InitializationCompleted
        {
            add { _initializationCompleted += value; }
            remove { _initializationCompleted -= value; }
        }


        public void OnInitializationCompleted()
        {
            var eventhandlers = _initializationCompleted;
            eventhandlers.Invoke(this, new EventArgs());
        }


    }
}
