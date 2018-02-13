using System;
using System.IO;

namespace Logging
{
    /// <summary>
    /// Logger that prints messages to file;
    /// </summary>
    public class FileLogger : Logger
    {
        /// <summary>
        /// File to print message.
        /// </summary>
        private StreamWriter file;
        /// <summary>
        /// Name of file to print messages
        /// </summary>
        private string parameter;
        /// <summary>
        /// Constructor that allow to set filename.
        /// Note that file is opened before first logging.
        /// </summary>
        /// <param name="parameter"></param>
        public FileLogger(string parameter)
        {
            this.parameter = parameter;
        }
        /// <summary>
        /// Default constructor. Sets filename as log.txt
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
        /// Writing message to file
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="type">Type of logged message.</param>
        protected override void Write(string message, LogType type)
        {
            Init();
            file.WriteLine(message);
        }
        /// <summary>
        /// Closing opened file.
        /// </summary>
        public override void Dispose()
        {
            file?.Dispose();
            file = null;
        }
    }
}
