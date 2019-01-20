using System;
using System.IO;

namespace Logging
{
    /// <summary>
    /// Logger that prints messages to file.
    /// </summary>
    public class FileLogger : Logger
    {
        private StreamWriter file;
        private string parameter;
        /// <summary>
        /// Initializes new instance of FileLogger.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if parameter is null or empty.</exception>
        /// <param name="parameter"></param>
        public FileLogger(string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentException("Parameter parameter can not be empty.", nameof(parameter));
            }

            this.parameter = parameter;
        }
        /// <summary>
        /// Initializes new instance of FileManager. Sets filename as log.txt
        /// Note that file is opened before first logging.
        /// </summary>
        public FileLogger() : this("log.txt")
        {
        }
        /// <summary>
        /// If file is not open, this method open it.
        /// </summary>
        public void Init()
        {
            if (file != null)
                return;
            file = new StreamWriter(parameter, true);
        }
        /// <summary>
        /// Writes message to file.
        /// Note that file is opened before first logging.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="type">Type of logged message.</param>
        protected override void Write(string message, LogType type)
        {
            Init();
            file.WriteLine(message);
        }
        /// <summary>
        /// Closes opened file.
        /// </summary>
        public override void Dispose()
        {
            file?.Dispose();
            file = null;
        }
    }
}
