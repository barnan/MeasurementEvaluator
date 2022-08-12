using FrameInterfaces;

namespace Frame.MessageHandler
{
    public class UIMessageControl : IUIMessageControl
    {
        private readonly IMyLogger _logger = PluginLoader.PluginLoader.GetLogger(nameof(UIMessageControl));

        public void AddMessage(string message)
        {
            AddMessage(message, MessageSeverityLevels.Info);
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
        public MessageEventArg(string data1, MessageSeverityLevels data2)
        {
            Data1 = data1;
            Data2 = data2;
        }

        public string Data1 { get; }

        public MessageSeverityLevels Data2 { get; }
    }
}
