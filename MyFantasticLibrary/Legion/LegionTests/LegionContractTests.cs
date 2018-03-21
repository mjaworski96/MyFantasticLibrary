using System;
using System.Diagnostics;
using System.IO;
using BasicTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegionTests
{
    [TestClass]
    public class LegionContractTests
    {
        [TestMethod]
        public void TestLegionTask()
        {
            DataIn dataIn = new DataIn() { A = 3, B = 4, Wait = 100 };
            AddAndWaitTask task = new AddAndWaitTask();
            Stopwatch stopwatch = Stopwatch.StartNew();
            DataOut dataOut = (DataOut)task.Run(dataIn);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Assert.AreEqual(7, dataOut.Result);
            Assert.IsTrue(stopwatch.ElapsedMilliseconds >= 100);
        }
        [TestMethod]
        public void TestLegionTaskWithStream()
        {
            DataIn dataIn = new DataIn();
            Stream streamIn = new MemoryStream();
            Stream streamOut = new MemoryStream();

            StreamWriter writerIn = new StreamWriter(streamIn);
            writerIn.WriteLine("-3,-5,150");
            writerIn.Flush();
            streamIn.Position = 0;
            dataIn.LoadFromStream(new StreamReader(streamIn));


            AddAndWaitTask task = new AddAndWaitTask();
            Stopwatch stopwatch = Stopwatch.StartNew();
            DataOut dataOut = (DataOut)task.Run(dataIn);
            stopwatch.Stop();

            StreamWriter writerOut = new StreamWriter(streamOut);
            dataOut.SaveToStream(writerOut);
            writerOut.Flush();
            streamOut.Position = 0;

            StreamReader readerOut = new StreamReader(streamOut);

            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Assert.AreEqual("-8", readerOut.ReadLine());
            Assert.IsTrue(stopwatch.ElapsedMilliseconds >= 150);
        }
    }
}
