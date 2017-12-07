using System;

namespace Logging
{
    public abstract class Logger: IDisposable
    {
        public abstract void Log(LogType type, string message, bool addUtcTime);
        
        public abstract void Dispose();

        protected string CreateLogMessage(LogType type, string message, bool addUtcTime)
        {
            string completeMessage = "";
            if (addUtcTime)
                completeMessage += DateTime.UtcNow + " ";
            completeMessage += type + ": " + message;
            return completeMessage;
        }

        
    }
}
