namespace Logging
{
    public class ConsoleLogger : Logger
    {
        public ConsoleLogger()
        {
        }

        public override void Dispose()
        {
            
        }

        public override void Log(LogType type, string message, bool addUtcTime)
        {
            System.Console.WriteLine(CreateLogMessage(type,message,addUtcTime));
        }
    }
}
