using System;

namespace Interfaces.Misc
{
    public interface IMessageControl
    {
        void AddMessage(string message);

        void AddMessage(string message, MessageLevels level);

        event EventHandler MessageReceived;

    }

}
