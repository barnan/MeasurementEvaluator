using Interfaces;
using Interfaces.Misc;
using System;
using System.Collections.Generic;

namespace MeasurementEvaluatorUIWPF.MessageControlUIWPF
{
    public class Message
    {
        public string MessageText { get; internal set; }

        public MessageLevels MessageLevel { get; internal set; }

    }



    public class MessageControl : IMessageControl
    {
        private List<Message> _messages = new List<Message>();
        private object _lockObj = new object();


        public void AddMessage(string message)
        {
            AddMessage(message, MessageLevels.Trace);
        }

        public void AddMessage(string message, MessageLevels level)
        {
            lock (_lockObj)
            {
                Message msg = new Message { MessageText = message, MessageLevel = level };

                _messages.Add(msg);

                MessageReceived?.Invoke(this, new EventArg<Message>(msg));
            }
        }

        public event EventHandler MessageReceived;
    }
}
