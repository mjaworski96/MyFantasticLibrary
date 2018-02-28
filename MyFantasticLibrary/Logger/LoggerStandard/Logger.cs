using System;

namespace Logging
{
    /// <summary>
    /// Base class for all loggers
    /// </summary>
    public abstract class Logger: IDisposable
    {
        /// <summary>
        /// Filter log messages. Only higer or eqals values are logged. 
        /// Default value is Information
        /// <seealso cref="LogType"/>
        /// </summary>
        public LogType Filter { get; set; } = LogType.Information;
        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="type">Type of logged message. If lower than Filter message will be ignored.</param>
        /// <param name="message">Message to be logged.</param>
        /// <param name="addUtcTime">If true UTC time will be added to message.</param>
        public void Log(LogType type, string message, bool addUtcTime)
        {
            if(type >= Filter)
            {
                Write(CreateLogMessage(type, message, addUtcTime), type);
            }
        }
        /// <summary>
        /// Logs message. Must be defined in child class.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="type">Type of logged message.</param>
        protected abstract void Write(string message, LogType type);
        /// <summary>
        /// Disposes all opened resources. 
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// Creates output message.
        /// </summary>
        /// <param name="type">Type of logged message. If lower than Filter message will be ignored.</param>
        /// <param name="message">Message to be logged.</param>
        /// <param name="addUtcTime">If true UTC time will be added to message.</param>
        /// <returns>Message with type and UTC time (optionally) that will be loggged.</returns>
        protected virtual string CreateLogMessage(LogType type, string message, bool addUtcTime)
        {
            string completeMessage = "";
            if (addUtcTime)
                completeMessage += DateTime.Now + " ";
            completeMessage += type + ": " + message;
            return completeMessage;
        }

        
    }
}
