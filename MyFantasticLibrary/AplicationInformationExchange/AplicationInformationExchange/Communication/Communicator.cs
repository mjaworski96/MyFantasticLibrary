using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using AplicationInformationExchange.Model;
using AplicationInformationExchange.Serialization;
using ConfigurationManager;

namespace AplicationInformationExchange.Communication
{
    /// <summary>
    /// Abstract socket communicator
    /// </summary>
    public abstract class Communicator
    {
        /// <summary>
        /// Used for serializing and deserializing while sending it via socket.
        /// </summary>
        protected ISerialization _Serializer = new JsonSerialization();
        /// <summary>
        /// IP address
        /// </summary>
        protected string _Address;
        /// <summary>
        /// Used TCP port
        /// </summary>
        protected int _Port;
        /// <summary>
        /// How many data can be received by once.
        /// </summary>
        protected int _BufferSize;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="port">TCP port</param>
        /// <param name="bufferSize">Bufer size</param>
        protected Communicator(string address, int port, int bufferSize = 10240)
        {
            _Address = address;
            _Port = port;
            _BufferSize = bufferSize;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configFilename">Path to filename with configuration</param>
        protected Communicator(string configFilename = "config.xml")
        {
            Configuration configuration = new Configuration(configFilename);
            _Address = configuration.GetString("aie.address");
            _Port = int.Parse(configuration.GetString("aie.port"));
            string bufferSize = configuration.GetString("aie.buffersize");
            if(string.IsNullOrWhiteSpace(bufferSize))
            {
                bufferSize = "10240";
            }
            _BufferSize = int.Parse(bufferSize);
        }
        /// <summary>
        /// Read one message
        /// </summary>
        /// <param name="client">Socket to get data</param>
        /// <returns>Received message</returns>

        protected Message ReadOne(Socket client)
        {
            List<byte> bytes = new List<byte>();
            byte[] buffer = new byte[_BufferSize];
            do
            {
                int received = client.Receive(buffer);
                for (int i = 0; i < received; i++)
                {
                    bytes.Add(buffer[i]);
                }
            } while (client.Available > 0);

            return _Serializer.Deserialize(bytes.ToArray());
        }
        /// <summary>
        /// Creates TCP socket.
        /// </summary>
        /// <param name="endPoint">Used ip endpoint</param>
        /// <param name="socket">Created socket</param>
        protected void CreateSocket(out IPEndPoint endPoint, out Socket socket)
        {
            IPAddress ip = IPAddress.Parse(_Address);
            endPoint = new IPEndPoint(ip, _Port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}