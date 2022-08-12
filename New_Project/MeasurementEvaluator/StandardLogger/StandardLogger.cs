using System.Diagnostics;
using System.Runtime.InteropServices;
using FrameInterfaces;
using NLog;

namespace StandardLogger
{
    public class StandardLogger : IMyLogger
    {
        private static Dictionary<string, IMyLogger> _loggers;
        private ILogger _logger;
        private const bool USE_CONSOLE = true;
        

        static StandardLogger()
        {
            _loggers = new Dictionary<string, IMyLogger>();

            HideConsole();
        }

        public StandardLogger(ILogger logger)
        {
            _logger = logger;
        }

        public static IMyLogger GetLogger(string name)
        {
            if (_loggers.ContainsKey(name))
            {
                return _loggers[name];
            }

            _loggers[name] = new StandardLogger(LogManager.GetLogger(name));

            return _loggers[name];
        }

        public string Trace(string inputMessage, Exception exception)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Trace(message);

            if (USE_CONSOLE)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message + Environment.NewLine);
                Console.ForegroundColor = previousColor;
            }

            return message;
        }

        public string Info(string inputMessage, Exception exception)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Info(message);

            if (USE_CONSOLE)
            {
                Console.WriteLine(message + Environment.NewLine);
            }

            return message;
        }

        public string Debug(string inputMessage, Exception exception = null)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Debug(message);

            if (USE_CONSOLE)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(message + Environment.NewLine);
                Console.ForegroundColor = previousColor;
            }

            return message;
        }

        public string Warning(string inputMessage, Exception exception = null)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Warn(message);

            if (USE_CONSOLE)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message + Environment.NewLine);
                Console.ForegroundColor = previousColor;
            }

            return message;
        }

        public string Error(string inputMessage, Exception exception = null)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Error(message);

            if (USE_CONSOLE)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message + Environment.NewLine);
                Console.ForegroundColor = previousColor;
            }

            return message;
        }

        public bool IsTraceEnabled => _logger.IsTraceEnabled;

        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        public bool IsWarningEnabled => _logger.IsWarnEnabled;

        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        #region Console Window

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        [Conditional("RELEASE")]
        private static void HideConsole()               // todo: test if works correctly
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }

        #endregion

    }
}