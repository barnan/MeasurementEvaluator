using Interfaces;
using Interfaces.Misc;
using MeasurementEvaluatorUI.Base;
using NLog;
using System;
using System.Collections.ObjectModel;

namespace MeasurementEvaluatorUIWPF.MessageControlUIWPF
{

    public class UIMessageControl : ViewModelBase, IUIMessageControl
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly object _lockObj = new object();


        public void AddMessage(string message)
        {
            AddMessage(message, MessageSeverityLevels.Trace);
        }

        public void AddMessage(string message, MessageSeverityLevels severityLevel)
        {
            lock (_lockObj)
            {
                Message msg = new Message { MessageText = message, MessageSeverityLevel = severityLevel };

                _messages.Add(msg);

                MessageReceived?.Invoke(this, new CustomEventArg<Message>(msg));

                if (_logger.IsTraceEnabled)
                {
                    _logger.Trace($"Message arrived. Text: {msg.MessageText}, Severity level: {msg.MessageSeverityLevel}");
                }
            }
        }

        public event EventHandler MessageReceived;

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }
    }



    public class Message
    {
        public string MessageText { get; internal set; }

        public MessageSeverityLevels MessageSeverityLevel { get; internal set; }
    }


}
