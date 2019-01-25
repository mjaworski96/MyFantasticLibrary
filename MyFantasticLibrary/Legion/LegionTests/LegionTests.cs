using LegionCore.Architecture;
using LegionCore.Architecture.Client;
using LegionCore.Architecture.Server;
using LegionCore.InMemoryCommunication;
using LegionCore.NetworkCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LegionTests
{
    [TestClass]
    public class LegionTests
    {
        private const string BASIC_TASK_VALID_RESULT = "Parameter initialization error: Input string was not in a correct format.\r\n1\r\n-1\r\n15\r\n-34\r\n35\r\n110\r\n10\r\n-374\r\n40\r\n";
        private const string ERROR_TASK_VALID_RESULT = "Parameter initialization error: Input string was not in a correct format.\r\nTask execution error: test\r\nTask execution error: test\r\nTask execution error: test\r\nTask execution error: test\r\nTask execution error: test\r\nTask execution error: test\r\nTask execution error: test\r\nTask execution error: test\r\nTask execution error: test\r\n";
        private void TestInMemory(string config, int validResultsCount, string validResult)
        {
            Tuple<Task, LegionServer> server = LegionServer.StartNew(config);
            IClientCommunicator communicator = new InMemoryClientCommunicator(server.Item2);
            Task client = RunClient(communicator, config);
            Task.WaitAll(server.Item1, client);
            CheckResults(validResultsCount, validResult);
        }
        private void TestNetwork(string config, int validResultsCount, string validResult)
        {
            LegionServer server = LegionServer.StartNew(config).Item2;
            NetworkServer serverManager = new NetworkServer(server, config);
            Task serverManagerTask = serverManager.Start();
            IClientCommunicator communicator = new NetworkClient(config);
            Task client = RunClient(communicator, config);
            Task.WaitAll(serverManagerTask, client);
            CheckResults(validResultsCount, validResult);
        }
        private void CheckResults(int validResultsCount, string validResult)
        {
            Assert.AreEqual(validResultsCount, File.ReadAllLines("data_out.txt").Length);
            string result = File.ReadAllText("ordered_data_out.txt");
            Assert.AreEqual(validResult, result);
        }
        [TestMethod, Timeout(20000)]
        public void TestInMemory5Workers()
        {
            TestInMemory("config_in_memory_5_workers.xml", 10, BASIC_TASK_VALID_RESULT);
        }
        [TestMethod, Timeout(20000)]
        public void TestInMemory10Workers()
        {
            TestInMemory("config_in_memory_10_workers.xml", 10, BASIC_TASK_VALID_RESULT);
        }
        [TestMethod, Timeout(20000)]
        public void TestInMemory20Workers()
        {
            TestInMemory("config_in_memory_20_workers.xml", 10, BASIC_TASK_VALID_RESULT);
        }
        [TestMethod, Timeout(60000)]
        public void TestInMemory1Worker()
        {
            TestInMemory("config_in_memory_1_worker.xml", 10, BASIC_TASK_VALID_RESULT);
        }
        [TestMethod, Timeout(20000)]
        public void TestNetwork5Workers()
        {
            TestNetwork("config_network_5_workers.xml", 10, BASIC_TASK_VALID_RESULT);
        }
        [TestMethod, Timeout(20000)]
        public void TestNetwork15Workers()
        {
            TestNetwork("config_network_15_workers.xml", 10, BASIC_TASK_VALID_RESULT);
        }
        [TestMethod, Timeout(20000)]
        public void TestErrorTask()
        {
            TestInMemory("config_error.xml", 10, ERROR_TASK_VALID_RESULT);
        }
        private async Task RunClient(IClientCommunicator communicator, string config)
        {
            LegionClient client = new LegionClient(communicator, config);
            await client.Run();

        }
    }
}
