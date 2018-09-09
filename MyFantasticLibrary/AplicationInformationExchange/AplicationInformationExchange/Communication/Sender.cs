using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using AplicationInformationExchange.Model;
using AplicationInformationExchange.Serialization;

namespace AplicationInformationExchange.Communication
{
    public class Sender: Communicator
    {
        public Sender(string address, int port) : base(address, port)
        {
        }

        public Message Send(Message message)
        {
            CreateSocket(out IPEndPoint endPoint, out Socket socket);
            socket.Connect(endPoint);
            socket.Send(_Serializer.Serialize(message));
            return ReadOne(socket, 10240);
        }
    }
}