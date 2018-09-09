using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using AplicationInformationExchange.Model;
using AplicationInformationExchange.Serialization;

namespace AplicationInformationExchange.Communication
{
    public abstract class Communicator
    {
        protected JsonSerialization _Serializer = new JsonSerialization();
        protected string _Address;
        protected int _Port;

        protected Communicator(string address, int port)
        {
            this._Address = address;
            this._Port = port;
        }

        protected Message ReadOne(Socket client, int bufferSize)
        {
            List<byte> bytes = new List<byte>();
            byte[] buffer = new byte[bufferSize];
            int received = client.Receive(buffer);
            for (int i = 0; i < received; i++)
            {
                bytes.Add(buffer[i]);
            }
            return _Serializer.Deserialize(bytes.ToArray());
        }
        protected void CreateSocket(out IPEndPoint endPoint, out Socket socket)
        {
            IPAddress ip = IPAddress.Parse(_Address);
            endPoint = new IPEndPoint(ip, _Port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}