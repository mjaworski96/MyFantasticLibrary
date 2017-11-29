using System;

namespace Logging
{
    public abstract class Logger: IDisposable
    {
        protected string parameter;
        public abstract void Log(LogType type, string message, bool addUtcTime);
        
        public abstract void Dispose();

        public Logger(string parameter)
        {
            this.parameter = parameter;
        }
    }
}
