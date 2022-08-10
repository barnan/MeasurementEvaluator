
namespace FrameInterfaces
{
    public interface IMyLogger
    {
        string Trace(string input, Exception exception = null);
        string Info(string input, Exception exception = null);
        string Debug(string input, Exception exception = null);
        string Warning(string input, Exception exception = null);
        string Error(string input, Exception exception = null);

        bool IsTraceEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsWarningEnabled { get; }
        bool IsErrorEnabled { get; }
    }
}
