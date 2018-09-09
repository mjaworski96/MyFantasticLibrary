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
        private Func<Message, Message> messageFactory;
        private Func<bool> endCondition;
        private int bufferSize;
        private int queueMaxSize;

        public Receiver(string address, int port, Func<Message, Message> messageFactory, Func<bool> endCondition = null, 
            int bufferSize = 10240, int queueMaxSize = 10)
                : base(address, port)
        {
            this.messageFactory = messageFactory;
            this.endCondition = endCondition;
            this.bufferSize = bufferSize;
            this.queueMaxSize = queueMaxSize;
        }

        public Message ReceiveOne()
        {
            Socket socket = Connect();
            Socket client = socket.Accept();
            Message result = ReadOne(client, bufferSize);
            client.Send(_Serializer.Serialize(messageFactory.Invoke(result)));
            return result;
        }
        public void ReceiveAll()
        {
            Socket socket = Connect();
            while ( !(endCondition?.Invoke() ?? false))
            {
                Socket client = socket.Accept();
                Message message = ReadOne(client, bufferSize);      
                client.Send(_Serializer.Serialize(messageFactory.Invoke(message)));     
            }      
        }
        private Socket Connect()
        {
            CreateSocket(out IPEndPoint endPoint, out Socket socket);
            socket.Bind(endPoint);
            socket.Listen(queueMaxSize);
            return socket;
        }
    }
}