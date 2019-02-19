using Interfaces;
using Interfaces.Misc;
using MeasurementEvaluatorUI.Base;
using System;
using System.Collections.ObjectModel;

namespace MeasurementEvaluatorUIWPF.MessageControlUIWPF
{
    public class Message
    {
        public string MessageText { get; internal set; }

        public MessageSeverityLevels MessageSeverityLevel { get; internal set; }

    }



    public class MessageControl : ViewModelBase, IMessageControl
    {

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

                MessageReceived?.Invoke(this, new EventArg<Message>(msg));
            }
        }

        public event EventHandler MessageReceived;

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
            }
        }



    }
}
