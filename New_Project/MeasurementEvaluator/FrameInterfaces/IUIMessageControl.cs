
namespace FrameInterfaces
{
    public interface IUIMessageControl
    {
        void AddMessage(string message);

        void AddMessage(string message, MessageSeverityLevels severityLevel);

        event EventHandler MessageReceived;
    }
}
