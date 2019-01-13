using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using AplicationInformationExchange.Model;
using AplicationInformationExchange.Serialization;

namespace AplicationInformationExchange.Communication
{
    public class Receiver: Communicator
    {
        private Func<Message, Message> _MessageFactory;
        private Func<bool> _EndCondition;
        private int _QueueMaxSize;

        public Receiver(string address, int port, Func<Message, Message> messageFactory, Func<bool> endCondition = null, 
            int bufferSize = 10240, int queueMaxSize = 10)
                : base(address, port, bufferSize)
        {
            this._MessageFactory = messageFactory;
            this._EndCondition = endCondition;
            this._QueueMaxSize = queueMaxSize;
        }

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
        private Socket Connect()
        {
            CreateSocket(out IPEndPoint endPoint, out Socket socket);
            socket.Bind(endPoint);
            socket.Listen(_QueueMaxSize);
            return socket;
        }
    }
}