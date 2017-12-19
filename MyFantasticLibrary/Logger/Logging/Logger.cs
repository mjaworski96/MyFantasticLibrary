using System;

namespace Logging
{
    public abstract class Logger: IDisposable
    {
        public LogType Filter { get; set; } = LogType.Information;
        public void Log(LogType type, string message, bool addUtcTime)
        {
            if(type >= Filter)
            {
                Write(CreateLogMessage(type, message, addUtcTime), type);
            }
        }
        protected abstract void Write(string message, LogType type);
        public abstract void Dispose();

        private string CreateLogMessage(LogType type, string message, bool addUtcTime)
        {
            string completeMessage = "";
            if (addUtcTime)
                completeMessage += DateTime.Now + " ";
            completeMessage += type + ": " + message;
            return completeMessage;
        }

        
    }
}
