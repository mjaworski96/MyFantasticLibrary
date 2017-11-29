using System;
using System.IO;

namespace Logging
{
    public class FileLogger : Logger
    {
        private StreamWriter file;
        private bool init;

        public FileLogger(string parameter):base(parameter)
        {

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
            string completeMessage = ""; ;
            if (addUtcTime)
                completeMessage += DateTime.UtcNow + " ";
            completeMessage += type + ": " + message; 
            file.WriteLine(completeMessage);
        }
        public override void Dispose()
        {
            file.Dispose();
        }
    }
}
