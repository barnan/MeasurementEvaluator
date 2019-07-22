using NLog;
using System;

namespace Frame.MessageHandler
{

    public class UIMessageControl : IUIMessageControl
    {
        private readonly ILogger _logger = LogManager.GetLogger(nameof(UIMessageControl));


        public void AddMessage(string message)
        {
            AddMessage(message, MessageSeverityLevels.Trace);
        }

        public void AddMessage(string message, MessageSeverityLevels severityLevel)
        {
            MessageReceived?.Invoke(this, new MessageEventArg<string, MessageSeverityLevels>(message, severityLevel));

            if (_logger.IsTraceEnabled)
            {
                _logger.Trace($"Message received. Text: {message}, Severity level: {severityLevel}");
            }
        }
        public event EventHandler MessageReceived;
    }


    public interface IUIMessageControl
    {
        void AddMessage(string message);

        void AddMessage(string message, MessageSeverityLevels severityLevel);

        event EventHandler MessageReceived;
    }

    public enum MessageSeverityLevels
    {
        Trace,
        Info,
        Warning,
        Error
    }

    public class MessageEventArg<T, E> : EventArgs
    {
        private readonly T _data1;
        private readonly E _data2;

        public MessageEventArg(T data1, E data2)
        {
            _data1 = data1;
            _data2 = data2;
        }

        public T Data1 => _data1;
        public E Data2 => _data2;
    }
}
