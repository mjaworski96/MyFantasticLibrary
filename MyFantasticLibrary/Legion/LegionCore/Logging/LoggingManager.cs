using Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LegionCore.Logging
{

    //TODO:
    //Make LoggingManager Shared Singleton
    public static class LoggingManager
    {
        private static Logger _Logger;
        private static Task _LoggerTask;
        private static Queue<LoggingInformation> _Queue;
        private static Semaphore _Semaphore;

        static LoggingManager()
        {
            _Logger = LoggerManager.Default;
            _Queue = new Queue<LoggingInformation>();
            _Semaphore = new Semaphore(0, Int32.MaxValue);
            _LoggerTask = Task.Run(() => LogTask());  
        }

        public static void LogInformation(string msg, bool addUtcTime = true)
        {
            AddToQueue(LogType.Information, msg, addUtcTime);
        }
        public static void LogWarning(string msg, bool addUtcTime = true)
        {
            AddToQueue(LogType.Warning, msg, addUtcTime);
        }
        public static void LogError(LogType type, string msg, bool addUtcTime = true)
        {
            AddToQueue(LogType.Error, msg, addUtcTime);
        }
        public static void LogCritical(string msg, bool addUtcTime = true)
        {
            AddToQueue(LogType.Critical, msg, addUtcTime);
        }

        private static void AddToQueue(LogType type, string msg, bool addUtcTime)
        {
            LoggingInformation information = new LoggingInformation(type, msg, addUtcTime);
            lock(_Queue)
            { 
                _Queue.Enqueue(information);
                _Semaphore.Release();
            }

        }
        private static void LogTask()
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            while (true)
            {
                _Semaphore.WaitOne();
                lock(_Queue)
                { 
                    LoggingInformation information = _Queue.Dequeue();
                    _Logger.Log(information.LogType, information.Message, information.AddUtcTime);
                }
            }
        }
    }
}
