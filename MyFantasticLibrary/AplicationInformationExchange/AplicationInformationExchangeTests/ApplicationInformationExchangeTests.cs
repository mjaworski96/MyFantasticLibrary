using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AplicationInformationExchange.Communication;
using AplicationInformationExchange.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AplicationInformationExchangeTests
{
    [TestClass]
    public class ApplicationInformationExchangeTests
    {
        private Message Send(Message message, int port)
        {
            Sender sender = new Sender("127.0.0.1", port);
            return sender.Send(message);
        }
        private Message ReceiveOne(int port, Func<Message, Message> okResult, Func<bool> end)
        {
            Receiver receiver = new Receiver("127.0.0.1", port, okResult, end);
            return receiver.ReceiveOne();
        }
        private void ReceiveMultiple(int port, Func<Message, Message> okResult, Func<bool> end)
        {
            Receiver receiver = new Receiver("127.0.0.1", port, okResult, end);
            receiver.ReceiveAll();
        }
        [TestMethod]
        public void TestSendOnePage()
        {
            int index = 0;
            Func<Message, Message> okResult = (m) =>
            {
                index++;
                return Message.WithEmptyBody(200, 0);
            };
            Func<bool> end = () => index >= 1;
            Message result = null;
            Task receiverTask = Task.Run(() =>
            {
                result = ReceiveOne(4000, okResult, end);
            });
            Thread.Sleep(100);

            Task senderTask = Task.Run(() =>
            {
                Message message = Message.WithStringContent("page1", "TEST message", 200, 100);
                Send(message, 4000);
            });
            Task.WaitAll(receiverTask, senderTask);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Body.PagesCount);
            Assert.AreEqual("page1", result.Body.GetPage(0).Name);
            Assert.AreEqual("TEST message", result.Body.GetPage(0).StringContent);
            Assert.AreEqual(200, result.Header.StatusCode);
            Assert.AreEqual(100, result.Header.OperationCode);
        }
        [TestMethod]
        public void TestSendTwoPages()
        {
            int index = 0;
            Func<Message, Message> okResult = (m) =>
            {
                index++;
                return Message.WithEmptyBody(200, 0);
            };
            Func<bool> end =
            () => index >= 1;
            Message result = null;
            Task receiverTask = Task.Run(() =>
            {
                result = ReceiveOne(4001, okResult, end);
            });
            Thread.Sleep(100);

            Task senderTask = Task.Run(() =>
            {
                Message message = Message.WithStringContent("page1", "TEST message", 200, 100);
                message.AddPage(BodyPage.FromString("page2", "TEST2"));
                Send(message, 4001);
            });
            Task.WaitAll(receiverTask, senderTask);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Body.PagesCount);
            Assert.AreEqual("page1", result.Body.GetPage(0).Name);
            Assert.AreEqual("TEST message", result.Body.GetPage(0).StringContent);
            Assert.AreEqual("page2", result.Body.GetPage(1).Name);
            Assert.AreEqual("TEST2", result.Body.GetPage(1).StringContent);
            Assert.AreEqual(200, result.Header.StatusCode);
            Assert.AreEqual(100, result.Header.OperationCode);
        }
        [TestMethod]
        public void TestSendTwoMessages()
        {
            int index = 0;
            List<Message> result = new List<Message>(2);
            Func<Message, Message> okResult = (m) =>
            {
                index++;
                result.Add(m);
                return Message.WithEmptyBody(200, 0);
            };
            Func<bool> end =
            () => index >= 2;
            
            Task receiverTask = Task.Run(() =>
            {
               ReceiveMultiple(4002, okResult, end);
            });
            Thread.Sleep(100);

            Task senderTask = Task.Run(() =>
            {
                Message message = Message.WithStringContent("page1", "TEST message", 200, 100);
                Send(message, 4002);
                message = Message.WithStringContent("page 1", "TEST message2", 201, 101);
                Send(message, 4002);
            });
            Task.WaitAll(receiverTask, senderTask);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            
            Assert.AreEqual(1, result[0].Body.PagesCount);
            Assert.AreEqual("page1", result[0].Body.GetPage(0).Name);
            Assert.AreEqual("TEST message", result[0].Body.GetPage(0).StringContent);
            Assert.AreEqual(200, result[0].Header.StatusCode);
            Assert.AreEqual(100, result[0].Header.OperationCode);

            Assert.AreEqual(1, result[1].Body.PagesCount);
            Assert.AreEqual("page 1", result[1].Body.GetPage(0).Name);
            Assert.AreEqual("TEST message2", result[1].Body.GetPage(0).StringContent);
            Assert.AreEqual(201, result[1].Header.StatusCode);
            Assert.AreEqual(101, result[1].Header.OperationCode);
        }
        [TestMethod]
        public void TestSendObject()
        {
            int index = 0;
            Func<Message, Message> okResult = (m) =>
            {
                index++;
                return Message.WithEmptyBody(200, 0);
            };
            Func<bool> end = () => index >= 1;
            Message result = null;
            Task receiverTask = Task.Run(() =>
            {
                result = ReceiveOne(4003, okResult, end);
            });
            Thread.Sleep(100);

            Task senderTask = Task.Run(() =>
            {
                Message message = Message.FromObject("page1", new TestClass(1, "x"), 200, 100);
                Send(message, 4003);
            });
            Task.WaitAll(receiverTask, senderTask);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Body.PagesCount);
            
            TestClass responseObject = result.Body.GetPage(0).ToObject<TestClass>();

            Assert.AreEqual(1, responseObject.Number);
            Assert.AreEqual("x", responseObject.Text);
            Assert.AreEqual(200, result.Header.StatusCode);
            Assert.AreEqual(100, result.Header.OperationCode);
        }
        [TestMethod]
        public void TestSendResponse()
        {
            int index = 0;
            Func<Message, Message> okResult = (m) =>
            {
                index++;
                return Message.WithEmptyBody(200, 0);
            };
            Func<bool> end = () => index >= 1;
            Message result = null;
            Task receiverTask = Task.Run(() =>
            {
                ReceiveOne(4004, okResult, end);
            });
            Thread.Sleep(100);

            Task senderTask = Task.Run(() =>
            {
                Message message = Message.WithEmptyBody(100, 300);
                result = Send(message, 4004);
            });
            Task.WaitAll(receiverTask, senderTask);
            Assert.IsNotNull(result); 

            Assert.IsTrue(result.Body.IsEmpty);
            Assert.AreEqual(0, result.Body.PagesCount);
            Assert.AreEqual(200, result.Header.StatusCode);
            Assert.AreEqual(0, result.Header.OperationCode);
        }
        [TestMethod]
        public void TestTwoClients()
        {
            int index = 0;
            List<Message> result = new List<Message>();
            Func<Message, Message> okResult = (m) =>
            {
                index++;
                result.Add(m);
                return Message.WithEmptyBody(200, 0);
            };
            Func<bool> end = () => index >= 2;
            
            Task receiverTask = Task.Run(() =>
            {
                ReceiveMultiple(4005, okResult, end);
            });
            Thread.Sleep(100);

            Task sender1Task = Task.Run(() =>
            {
                Message message = Message.WithEmptyBody(100, 500);
                Send(message, 4005);
            });
            Task sender2Task = Task.Run(() =>
            {
                Message message = Message.WithEmptyBody(200, 400);
                Send(message, 4005);
            });
            Task.WaitAll(receiverTask, sender1Task, sender2Task);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(300, result[0].Header.StatusCode + result[1].Header.StatusCode);
            Assert.AreEqual(900, result[0].Header.OperationCode + result[1].Header.OperationCode);
        }
    }
}
