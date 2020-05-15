using Frame.PluginLoader.Interfaces;
using System;

namespace MeasurementEvaluatorUIWPF.MessageControlUI
{
    public class Factory : IPluginFactory
    {
        private MessageControlUIWPF _messageControlUI;

        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(MessageControlUIWPF)))
            {
                return _messageControlUI ?? (_messageControlUI = new MessageControlUIWPF(new MessageControlParameters()));
            }

            return null;
        }
    }
}
