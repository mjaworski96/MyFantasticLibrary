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

        public FileLogger() : this("log.txt")
        {
        }

        public void Init()
        {
            if (init)
                return;
            file = new StreamWriter(parameter, true);
            init = true;
        }

        protected override void Write(string message)
        {
            Init();
            file.WriteLine(message);
        }
        public override void Dispose()
        {
            file.Dispose();
            init = false;
        }
    }
}
