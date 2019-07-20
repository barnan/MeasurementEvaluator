using Interfaces;
using Interfaces.Misc;
using MeasurementEvaluatorUI.Base;
using NLog;
using System;
using System.Collections.ObjectModel;

namespace MeasurementEvaluatorUIWPF.MessageControlUI
{

    public class UIMessageControl : ViewModelBase, IUIMessageControl
    {
        private readonly ILogger _logger = LogManager.GetLogger(nameof(UIMessageControl));
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

                Messages.Add(msg);

                MessageReceived?.Invoke(this, new CustomEventArg<Message>(msg));

                if (_logger.IsTraceEnabled)
                {
                    _logger.Trace($"Message received. Text: {msg.MessageText}, Severity level: {msg.MessageSeverityLevel}");
                }
            }
        }

        public event EventHandler MessageReceived;

        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }
    }



    public class Message
    {
        public string MessageText { get; set; }

        public MessageSeverityLevels MessageSeverityLevel { get; set; }
    }


}
