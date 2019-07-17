using Interfaces.Misc;
using System;

namespace MeasurementEvaluatorUIWPF.Base
{
    public class ParameterBase : INamed
    {
        public string Name { get; internal set; }


        // to call the initialization completed for all ui components
        private static event EventHandler _initializationCompleted;
        public event EventHandler InitializationCompleted
        {
            add => _initializationCompleted += value;
            remove => _initializationCompleted -= value;
        }

        public void OnInitializationCompleted()
        {
            var eventhandlers = _initializationCompleted;
            eventhandlers?.Invoke(this, new EventArgs());
        }


        // to call the closed for all ui components
        private static event EventHandler _closed;
        public event EventHandler Closed
        {
            add => _closed += value;
            remove => _closed -= value;
        }

        public void OnClosed()
        {
            var eventhandlers = _initializationCompleted;
            eventhandlers?.Invoke(this, new EventArgs());
        }

    }
}
