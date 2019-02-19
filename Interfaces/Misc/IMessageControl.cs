using System;

namespace Interfaces.Misc
{
    public interface IMessageControl
    {
        void AddMessage(string message);

        void AddMessage(string message, MessageSeverityLevels severityLevel);

        event EventHandler MessageReceived;

    }

}
