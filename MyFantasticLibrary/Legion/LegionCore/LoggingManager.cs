using Logging;
using System.Threading.Tasks;

namespace LegionCore
{
    public static class LoggingManager
    {

        private static Logger _logger;

        static LoggingManager()
        {
            _logger = LoggerManager.Default;
        }

        public static void LogInformation(string msg, bool addUtcTime = true)
        {
            Task.Run(() => LogTask(LogType.Information, msg, addUtcTime));
        }
        public static void LogWarning(string msg, bool addUtcTime = true)
        {
            Task.Run(() => LogTask(LogType.Warning, msg, addUtcTime));
        }
        public static void LogError(LogType type, string msg, bool addUtcTime = true)
        {
            Task.Run(() => LogTask(LogType.Error, msg, addUtcTime));
        }
        public static void LogCritical(string msg, bool addUtcTime = true)
        {
            Task.Run(() => LogTask(LogType.Critical, msg, addUtcTime));
        }
        private static void LogTask(LogType type, string msg, bool addUtcTime)
        {
            lock(_logger)
            {
                _logger.Log(type, msg, addUtcTime);
            }
        }
    }
}
