namespace Logging
{
    /// <summary>
    /// Type of logged message.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Message is information.
        /// </summary>
        Information,
        /// <summary>
        /// Messages informs about warning.
        /// </summary>
        Warning,
        /// <summary>
        ///  Messages informs about error.
        /// </summary>
        Error,
        /// <summary>
        /// Messages informs about critical event;
        /// </summary>
        Critical,
    }
}
