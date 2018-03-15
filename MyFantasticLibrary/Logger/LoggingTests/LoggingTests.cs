using System;
using Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoggingTests
{
    [TestClass]
    public class LoggingTests
    {
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ConsoleLoggerEmptyParameter()
        {
            Logger logger = new ConsoleLogger("");
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ConsoleLoggerNullParameter()
        {
            Logger logger = new ConsoleLogger(null);
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void FileLoggerEmptyParameter()
        {
            Logger logger = new FileLogger("");
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void FileLoggerNullParameter()
        {
            Logger logger = new FileLogger(null);
        }
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ConsoleLoggerLogEmptyParameter()
        {
            Logger logger = new ConsoleLogger();
            logger.Log(LogType.Information, null, true);
        }
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void FileLoggerLogEmptyParameter()
        {
            Logger logger = new FileLogger();
            logger.Log(LogType.Information, null, true);
        }
    }
}
