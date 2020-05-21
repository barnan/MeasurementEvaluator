using MyFrameWork.Interfaces;
using System;

namespace MyFrameWork.Loggers
{
    internal class LoggerToConsole : IMyLogger
    {
        private readonly string _name;
        private readonly ConsoleColor _CONSOLECOLOR_FATAL = ConsoleColor.DarkRed;
        private readonly ConsoleColor _CONSOLECOLOR_ERROR = ConsoleColor.Red;
        private readonly ConsoleColor _CONSOLECOLOR_WARNING = ConsoleColor.Yellow;
        private readonly ConsoleColor _CONSOLECOLOR_INFO;
        private readonly ConsoleColor _CONSOLECOLOR_DEBUG = ConsoleColor.DarkGreen;
        private readonly ConsoleColor _CONSOLECOLOR_TRACE = ConsoleColor.DarkGray;
        private readonly ConsoleColor _originalColor = Console.ForegroundColor;

        public LoggerToConsole(string name)
        {
            _name = name;
            _CONSOLECOLOR_INFO = _originalColor;
        }


        public string Trace(string message)
        {
            Console.WriteLine($"{_name}-{message}", _CONSOLECOLOR_TRACE);
            return message;
        }

        public string Debug(string message)
        {
            Console.WriteLine($"{_name}-{message}", _CONSOLECOLOR_DEBUG);
            return message;
        }

        public string Info(string message)
        {
            Console.WriteLine($"{_name}-{message}", _CONSOLECOLOR_INFO);
            return message;
        }

        public string Warning(string message)
        {
            Console.WriteLine($"{_name}-{message}", _CONSOLECOLOR_WARNING);
            return message;
        }

        public string Error(string message)
        {
            Console.WriteLine($"{_name}-{message}", _CONSOLECOLOR_ERROR);
            return message;
        }

        public string Fatal(string message)
        {

            Console.WriteLine($"{_name}-{message}", _CONSOLECOLOR_FATAL);
            return message;
        }

        public bool IsTraceEnabled() => true;

        public bool IsDebugEnabled() => true;

        public bool IsInfoEnabled() => true;

        public bool IsWarningEnabled() => true;

        public bool IsErrorEnabled() => true;

        public bool IsFatalEnabled() => true;
    }
}
