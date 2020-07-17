namespace MyFrameWork.Interfaces
{

    public enum FrameLoggerLevels
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public interface IMyLogger
    {
        string Trace(string message);
        string Debug(string message);
        string Info(string message);
        string Warning(string message);
        string Error(string message);
        string Fatal(string message);

        bool IsTraceEnabled();
        bool IsDebugEnabled();
        bool IsInfoEnabled();
        bool IsWarningEnabled();
        bool IsErrorEnabled();
        bool IsFatalEnabled();

    }
}
