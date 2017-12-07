using System;
using System.IO;

namespace Logging
{
    public class FileLogger : Logger
    {
        private StreamWriter file;
        private bool init;

        private string parameter;
        public FileLogger(string parameter)
        {
            this.parameter = parameter;
        }

        public void Init()
        {
            if (init)
                return;
            file = new StreamWriter(parameter, true);
            init = true;
        }

        public override void Log(LogType type, string message, bool addUtcTime)
        {
            Init();
            file.WriteLine(CreateLogMessage(type, message, addUtcTime));
        }
        public override void Dispose()
        {
            file.Dispose();
        }
    }
}
