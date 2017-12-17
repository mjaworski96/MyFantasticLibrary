using System;

namespace Logging
{
    public class ConsoleLogger : Logger
    {
        private ConsoleColor _color;
        public ConsoleLogger()
        {
        }
        public ConsoleLogger(string param)
        {
            _color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), param);
        }
        public override void Dispose()
        {
            
        }

        protected override void Write(string message)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = _color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}
