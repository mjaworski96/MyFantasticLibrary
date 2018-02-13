using System;
using System.Collections.Generic;

namespace Logging
{
    /// <summary>
    /// Logger that print messages to standard output.
    /// </summary>
    public class ConsoleLogger : Logger
    {
        /// <summary>
        /// Colors assigned to standard outputs.
        /// </summary>
        private Dictionary<LogType, ConsoleColor> _colors;
        /// <summary>
        /// Default constructor. Output color will not be changed.
        /// </summary>
        public ConsoleLogger()
        {
            _colors = new Dictionary<LogType, ConsoleColor>();
        }
        /// <summary>
        /// Constructor that set color of messages;
        /// </summary>
        /// <param name="param">
        /// Color or colors for messages depending on type of message.
        /// Each color must be compatible with <see cref="ConsoleColor"/>
        /// There are 3 formats of colors:
        /// 1. color - sets color for all types of message.
        /// 2. color1;color2 - sets color1 for Information and color2 for Warning, Error and Critical.
        /// 3. color1;color2;color3 - sets color1 for Information and color2 for Warning and color3 for Error and Critical.
        /// 4. color1;color2;color3 - sets color1 for Information and color2 for Warning and color3 for Error and color 4 for Critical.
        /// </param>
        public ConsoleLogger(string param) : this()
        {
            string[] colors = param.Split(';');
            if(colors.Length == 1)
            {
                _colors.Add(LogType.Information, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Warning, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Error, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Critical, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
            }
            else if (colors.Length == 2)
            {
                _colors.Add(LogType.Information, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Warning, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[1]));
                _colors.Add(LogType.Error, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[1]));
                _colors.Add(LogType.Critical, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[1]));
            }
            else if (colors.Length == 3)
            {
                _colors.Add(LogType.Information, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Warning, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[1]));
                _colors.Add(LogType.Error, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[2]));
                _colors.Add(LogType.Critical, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[2]));
            }
            else
            {
                _colors.Add(LogType.Information, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Warning, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[1]));
                _colors.Add(LogType.Error, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[2]));
                _colors.Add(LogType.Critical, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[3]));
            }
        }
        /// <summary>
        /// Definition of Dispose. Does nothing.
        /// </summary>
        public override void Dispose()
        {
            
        }
        /// <summary>
        /// Print message to standard output. If color for messages are defined, messages are printed in
        /// these colors. Default colors of Console is reset.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="type">Type of logged message.</param>
        protected override void Write(string message, LogType type)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            if(_colors.Count > 0) Console.ForegroundColor = _colors[type];
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}
