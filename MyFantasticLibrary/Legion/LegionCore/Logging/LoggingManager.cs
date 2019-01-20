using Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LegionCore.Logging
{
    /// <summary>
    /// Logging manager
    /// </summary>
    public class LoggingManager
    {
        private static volatile LoggingManager _Instance;
        private static object _SyncRoot = new Object();
        /// <summary>
        /// Gets singleton logger instance
        /// </summary>
        public static LoggingManager Instance
        {
            get
            {
                if(_Instance == null)
                {
                    lock(_SyncRoot)
                    {
                        if (_Instance == null)
                            _Instance = new LoggingManager();
                    }
                }

                return _Instance;
            }
        }

        private Logger _Logger;
        private Task _LoggerTask;
        private Queue<LoggingInformation> _Queue;
        private Semaphore _Semaphore;

        private LoggingManager()
        {
            _Logger = LoggerManager.Default;
            _Queue = new Queue<LoggingInformation>();
            _Semaphore = new Semaphore(0, Int32.MaxValue);
            _LoggerTask = Task.Run(() => LogTask());  
        }
        /// <summary>
        /// Log message with type information
        /// </summary>
        /// <param name="msg">Message to log</param>
        /// <param name="addUtcTime">If true, utc time will be added to message</param>
        public void LogInformation(string msg, bool addUtcTime = true)
        {
            AddToQueue(LogType.Information, msg, addUtcTime);
        }
        /// <summary>
        /// Log message with type warning
        /// </summary>
        /// <param name="msg">Message to log</param>
        /// <param name="addUtcTime">If true, utc time will be added to message</param>
        public void LogWarning(string msg, bool addUtcTime = true)
        {
            AddToQueue(LogType.Warning, msg, addUtcTime);
        }
        /// <summary>
        /// Log message with type error
        /// </summary>
        /// <param name="msg">Message to log</param>
        /// <param name="addUtcTime">If true, utc time will be added to message</param>
        public void LogError(string msg, bool addUtcTime = true)
        {
            AddToQueue(LogType.Error, msg, addUtcTime);
        }
        /// <summary>
        /// Log message with type critical
        /// </summary>
        /// <param name="msg">Message to log</param>
        /// <param name="addUtcTime">If true, utc time will be added to message</param>
        public void LogCritical(string msg, bool addUtcTime = true)
        {
            AddToQueue(LogType.Critical, msg, addUtcTime);
        }

        private void AddToQueue(LogType type, string msg, bool addUtcTime)
        {
            LoggingInformation information = new LoggingInformation(type, msg, addUtcTime);
            lock(_Queue)
            { 
                _Queue.Enqueue(information);
            }
            _Semaphore.Release();
        }
        private void LogTask()
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            while (true)
            {
                _Semaphore.WaitOne();
                LoggingInformation information = null;
                lock (_Queue)
                { 
                    information = _Queue.Dequeue();
                }
                if(information != null)
                    _Logger.Log(information.LogType, information.Message, information.AddUtcTime);
            }
        }
    }
}
