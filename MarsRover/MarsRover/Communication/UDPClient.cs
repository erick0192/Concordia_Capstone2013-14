using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MarsRover.Communication
{
    public sealed class UDPClient
    {
        #region Attributes

        private const string REMOTE_IP = "127.0.0.1";

        #endregion

        #region Singleton Constructor/Properties

        //Critical area of the constructor is locked so that only one instance is created even in a multithreaded environment
        //http://msdn.microsoft.com/en-us/library/ff650316.aspx
        private static volatile UDPClient instance;
        private static object syncRoot = new Object();

        private UDPClient() {}

        public static UDPClient Instance
        {
            get 
            {
                if (instance == null) 
                {
                    lock (syncRoot) 
                    {
                        if (instance == null) 
                            instance = new UDPClient();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Methods

        public void SendCommand(string command, int port)
        {
            Thread t = new Thread(() => SendCommandUdp(command, port));
            t.Start();
        }

        private void SendCommandUdp(string command, int port)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress broadcast = IPAddress.Parse(REMOTE_IP);

            byte[] sendbuf = Encoding.ASCII.GetBytes(command);
            IPEndPoint ep = new IPEndPoint(broadcast, port);

            s.SendTo(sendbuf, ep);
        }

        #endregion
    }
}
