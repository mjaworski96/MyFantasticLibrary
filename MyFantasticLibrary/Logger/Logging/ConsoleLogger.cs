using System;
using System.Collections.Generic;

namespace Logging
{
    public class ConsoleLogger : Logger
    {
        private Dictionary<LogType, ConsoleColor> _colors;
        public ConsoleLogger()
        {
            _colors = new Dictionary<LogType, ConsoleColor>();
        }
        public ConsoleLogger(string param) : this()
        {
            string[] colors = param.Split(';');
            if(colors.Length == 1)
            {
                _colors.Add(LogType.Information, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Warning, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Error, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
            }
            else if (colors.Length == 2)
            {
                _colors.Add(LogType.Information, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Warning, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[1]));
                _colors.Add(LogType.Error, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[1]));
            }
            else
            {
                _colors.Add(LogType.Information, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[0]));
                _colors.Add(LogType.Warning, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[1]));
                _colors.Add(LogType.Error, (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colors[2]));
            }
        }
        public override void Dispose()
        {
            
        }

        protected override void Write(string message, LogType type)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            if(_colors.Count > 0) Console.ForegroundColor = _colors[type];
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}
