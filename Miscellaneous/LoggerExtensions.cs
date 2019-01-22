using NLog;
using System.Runtime.CompilerServices;

namespace Miscellaneous
{
    public static class LoggerExtensions
    {
        public static void MethodTrace(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Trace($"{callermember}-{message}");
        }

        public static void MethodDebug(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Debug($"{callermember}-{message}");
        }

        public static void MethodInfo(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Info($"{callermember}-{message}");
        }

        public static void MethodWarning(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Warn($"{callermember}-{message}");
        }

        public static void MethodError(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Error($"{callermember}-{message}");
        }

        public static void MethodFatal(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            logger.Fatal($"{callermember}-{message}");
        }




        public static void LogTrace(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsTraceEnabled)
            {
                logger.Trace($"{callermember}-{message}");
            }
        }

        public static void LogDebug(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug($"{callermember}-{message}");
            }
        }

        public static void LogInfo(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsInfoEnabled)
            {
                logger.Info($"{callermember}-{message}");
            }
        }

        public static void LogWarning(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsWarnEnabled)
            {
                logger.Warn($"{callermember}-{message}");
            }
        }

        public static void LogError(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Error($"{callermember}-{message}");
            }
        }

        public static void LogFatal(this ILogger logger, string message, [CallerMemberName] string callermember = null)
        {
            if (logger.IsFatalEnabled)
            {
                logger.Fatal($"{callermember}-{message}");
            }
        }



    }
}
