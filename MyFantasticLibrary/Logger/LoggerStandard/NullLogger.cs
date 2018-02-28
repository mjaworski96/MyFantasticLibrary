namespace Logging
{
    /// <summary>
    /// Logger that ignores all messages.
    /// </summary>
    public class NullLogger : Logger
    {
        /// <summary>
        /// Initializes new instance of NullLogger.
        /// </summary>
        public NullLogger()
        {
        }
        /// <summary>
        /// Initializes new instance of NullLogger.
        /// Implemented only for avoid errors with calling one parameter constuctor by LoggerConfiuration Create method.
        /// </summary>
        /// <param name="message"></param>
        public NullLogger(string message)
        {
        }
        /// <summary>
        /// Definition of Dispose. Does nothing.
        /// </summary>
        public override void Dispose()
        {
           
        }
        /// <summary>
        /// Definition of Write. Does nothing.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="type">Type of logged message.</param>
        protected override void Write(string message, LogType type)
        {
           
        }
    }
}
