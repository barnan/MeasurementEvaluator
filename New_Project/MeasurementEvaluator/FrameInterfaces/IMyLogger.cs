
namespace FrameInterfaces
{
    public interface IMyLogger
    {
        string Trace(string input, params object[] parameters);
        string Info(string input, params object[] parameters);
        string Debug(string input, params object[] parameters);
        string Warning(string input, params object[] parameters);
        string Error(string input, params object[] parameters);

        bool IsTraceEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsWarningEnabled { get; }
        bool IsErrorEnabled { get; }
    }
}
