using NLog;
using System.Runtime.CompilerServices;

namespace Miscellaneous
{
    public static class LoggerExtensions
    {
        // -----------------------------------------------------
        // Callermembername
        public static string MethodTrace(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Trace($"{callermember}-{message}");
            return message;
        }

        public static string MethodDebug(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Debug($"{callermember}-{message}");
            return message;
        }

        public static string MethodInfo(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Info($"{callermember}-{message}");
            return message;
        }

        public static string MethodWarning(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Warn($"{callermember}-{message}");
            return message;
        }

        public static string MethodError(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Error($"{callermember}-{message}");
            return message;
        }

        public static string MethodFatal(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Fatal($"{callermember}-{message}");
            return message;
        }


        // -----------------------------------------------------
        // checks the logging level + callermembername
        public static string LogMethodTrace(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsTraceEnabled)
            {
                logger.Trace($"{callermember}-{message}");
            }
            return message;
        }

        public static string LogMethodDebug(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug($"{callermember}-{message}");
            }
            return message;
        }

        public static string LogMethodInfo(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsInfoEnabled)
            {
                logger.Info($"{callermember}-{message}");
            }
            return message;
        }

        public static string LogMethodWarning(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsWarnEnabled)
            {
                logger.Warn($"{callermember}-{message}");
            }
            return message;
        }

        public static string LogMethodError(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Error($"{callermember}-{message}");
            }
            return message;
        }

        public static string LogMethodFatal(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsFatalEnabled)
            {
                logger.Fatal($"{callermember}-{message}");
            }
            return message;
        }


        // -----------------------------------------------------
        //only checks the logging level, NO callermembername
        public static string LogTrace(this ILogger logger, string message)
        {
            if (logger.IsTraceEnabled)
            {
                logger.Trace(message);
            }
            return message;
        }

        public static string LogDebug(this ILogger logger, string message)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message);
            }
            return message;
        }

        public static string LogInfo(this ILogger logger, string message)
        {
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
            return message;
        }

        public static string LogWarning(this ILogger logger, string message)
        {
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message);
            }
            return message;
        }

        public static string LogError(this ILogger logger, string message)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Error(message);
            }
            return message;
        }

        public static string LogFatal(this ILogger logger, string message)
        {
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message);
            }
            return message;
        }

    }
}
