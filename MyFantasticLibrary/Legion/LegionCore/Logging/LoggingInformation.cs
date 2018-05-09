using Logging;

namespace LegionCore.Logging
{
    class LoggingInformation
    {
        LogType _LogType;
        string _Message;
        bool _AddUtcTime;

        public LoggingInformation(LogType LogType, string message, bool addUtcTime)
        {
            _LogType = LogType;
            _Message = message;
            _AddUtcTime = addUtcTime;
        }

        public LogType LogType { get => _LogType; set => _LogType = value; }
        public string Message { get => _Message; set => _Message = value; }
        public bool AddUtcTime { get => _AddUtcTime; set => _AddUtcTime = value; }
    }
}
