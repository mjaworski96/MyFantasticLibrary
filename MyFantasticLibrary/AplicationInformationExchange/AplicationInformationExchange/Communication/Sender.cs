using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using AplicationInformationExchange.Model;
using AplicationInformationExchange.Serialization;

namespace AplicationInformationExchange.Communication
{
    /// <summary>
    /// Sender - can send information and receive response.
    /// </summary>
    public class Sender: Communicator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="port">TCP port</param>
        /// <param name="bufferSize">Buffer size</param>
        public Sender(string address, int port, int bufferSize = 10240) : base(address, port, bufferSize)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configFilePath">Path to config file</param>
        public Sender(string configFilePath): base(configFilePath) { }
        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>Response</returns>
        public Message Send(Message message)
        {
            CreateSocket(out IPEndPoint endPoint, out Socket socket);
            using (socket)
            {
                socket.Connect(endPoint);
                socket.Send(_Serializer.Serialize(message));
                return ReadOne(socket);
            }
        }
    }
}