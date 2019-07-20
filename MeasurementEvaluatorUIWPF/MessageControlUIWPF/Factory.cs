using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using System;

namespace MeasurementEvaluatorUIWPF.MessageControlUI
{
    public class Factory : IPluginFactory
    {
        private IUIMessageControl _messageControl;
        private MessageControlUIWPF _messageControlUI;

        public object Create(Type t, string name)
        {
            //if (t.IsAssignableFrom(typeof(IUIMessageControl)))
            //{
            //    if (_messageControl == null)
            //    {
            //        _messageControl = new UIMessageControl();
            //    }

            //    _messageControlUI.Dispatcher.Invoke(() =>
            //    {
            //        if (_messageControlUI != null && _messageControlUI.DataContext == null)
            //        {
            //            _messageControlUI.DataContext = _messageControl;
            //        }
            //    });
            //    return _messageControl;
            //}

            if (t.IsAssignableFrom(typeof(MessageControlUIWPF)))
            {
                if (_messageControl == null)
                {
                    _messageControl = new UIMessageControl();
                }

                if (_messageControlUI == null)
                {
                    _messageControlUI = new MessageControlUIWPF();

                    if (_messageControl != null)
                    {
                        _messageControlUI.DataContext = _messageControl;
                    }
                }
                return _messageControlUI;
            }

            if (t.IsAssignableFrom(typeof(IUIMessageControl)))
            {
                if (_messageControl == null)
                {
                    _messageControl = new UIMessageControl();
                }
                return _messageControl;
            }

            return null;
        }
    }
}
