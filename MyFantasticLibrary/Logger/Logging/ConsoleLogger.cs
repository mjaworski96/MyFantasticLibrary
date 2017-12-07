namespace Logging
{
    public class ConsoleLogger : Logger
    {
        public ConsoleLogger()
        {
        }
        public ConsoleLogger(string param)
        {
        }
        public override void Dispose()
        {
            
        }

        protected override void Write(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
