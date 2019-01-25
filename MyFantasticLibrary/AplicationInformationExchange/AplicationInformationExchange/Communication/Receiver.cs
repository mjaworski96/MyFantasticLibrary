using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using AplicationInformationExchange.Model;
using AplicationInformationExchange.Serialization;
using ConfigurationManager;

namespace AplicationInformationExchange.Communication
{
    /// <summary>
    /// Receiver - can receive information and send response.
    /// </summary>
    public class Receiver: Communicator
    {
        private Func<Message, Message> _MessageFactory;
        private Func<bool> _EndCondition;
        private int _QueueMaxSize;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">IP address of sender</param>
        /// <param name="port">TCP port of sender</param>
        /// <param name="messageFactory">Response message factory</param>
        /// <param name="endCondition">ReceiveAll() method end condition</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="queueMaxSize">Queue max size</param>
        public Receiver(string address, int port, Func<Message, Message> messageFactory, Func<bool> endCondition = null, 
            int bufferSize = 10240, int queueMaxSize = 100)
                : base(address, port, bufferSize)
        {
            this._MessageFactory = messageFactory;
            this._EndCondition = endCondition;
            this._QueueMaxSize = queueMaxSize;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configFilePath">PAth to file with configuration</param>
        /// <param name="messageFactory">Response message factory</param>
        /// <param name="endCondition">ReceiveAll() method end condition</param>
        public Receiver(string configFilePath, Func<Message, Message> messageFactory, Func<bool> endCondition = null)
                : base(configFilePath)
        {
            this._MessageFactory = messageFactory;
            this._EndCondition = endCondition;
            Configuration configuration = new Configuration(configFilePath);
            string queueSize = configuration.GetString("network.queue");
            if (string.IsNullOrWhiteSpace(queueSize))
            {
                queueSize = "100";
            }
            this._QueueMaxSize = int.Parse(queueSize);
        }
        /// <summary>
        /// Receives one <see cref="Message"/> from <see cref="Sender"/>
        /// </summary>
        /// <returns>Received <see cref="Message"/></returns>
        public Message ReceiveOne()
        {
            Socket socket = Connect();
            Socket client = socket.Accept();
            using (socket)
            {
                using (client)
                {
                    Message result = ReadOne(client);
                    client.Send(_Serializer.Serialize(_MessageFactory.Invoke(result)));
                    return result;
                }
            }
        }
        /// <summary>
        /// Receive all <see cref="Message"/> from <see cref="Sender"/>.
        /// Method ends when endCondition is true. This method use _MessageFactory.
        /// </summary>
        public void ReceiveAll()
        {
            Socket socket = Connect();
            using (socket)
            {
                while (!(_EndCondition?.Invoke() ?? false))
                {
                    Socket client = socket.Accept();
                    using (client)
                    {
                        Message message = ReadOne(client);
                        client.Send(_Serializer.Serialize(_MessageFactory.Invoke(message)));
                    }
                }
            }      
        }
        /// <summary>
        /// Connect with sender
        /// </summary>
        /// <returns></returns>
        private Socket Connect()
        {
            CreateSocket(out IPEndPoint endPoint, out Socket socket);
            socket.Bind(endPoint);
            socket.Listen(_QueueMaxSize);
            return socket;
        }
    }
}