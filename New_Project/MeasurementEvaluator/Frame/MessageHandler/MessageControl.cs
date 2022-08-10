using BaseClasses.Frame;
using FrameInterfaces;

namespace Frame.MessageHandler
{
    public class UIMessageControl : IUIMessageControl
    {
        private readonly IMyLogger _logger = PluginLoader.PluginLoader.GetLogger(nameof(UIMessageControl));

        public void AddMessage(string message)
        {
            AddMessage(message, MessageSeverityLevels.Trace);
        }

        public void AddMessage(string message, MessageSeverityLevels severityLevel)
        {
            EventHandler messageReceived = MessageReceived;
            messageReceived?.Invoke(this, new MessageEventArg(message, severityLevel));

            if (_logger.IsTraceEnabled)
            {
                _logger.Trace($"Message received. Text: {message}, Severity level: {severityLevel}");
            }
        }
        public event EventHandler MessageReceived;
    }

    public class MessageEventArg : EventArgs
    {
        private readonly string _data1;
        private readonly MessageSeverityLevels _data2;

        public MessageEventArg(string data1, MessageSeverityLevels data2)
        {
            _data1 = data1;
            _data2 = data2;
        }

        public string Data1 => _data1;
        public MessageSeverityLevels Data2 => _data2;
    }
}
