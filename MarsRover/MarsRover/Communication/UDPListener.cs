using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NLog;

namespace MarsRover.Communication
{
    public class UDPListener
    {
        private int port;
        private Logger logger;

        private UdpClient listener;
        private IPEndPoint groupEP;

        public UDPListener(int port)
        {
            this.port = port;
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        public void Initialize()
        {
            Thread t = new Thread(StartListening);
            t.IsBackground = true;
            t.Start();
        }

        private void StartListening()
        {
            listener = new UdpClient(port);
            groupEP = new IPEndPoint(IPAddress.Any, port);

            try
            {
                while (true)
                {
                    logger.Trace("Starting UDPListener on port " + port);
                    byte[] bytes = listener.Receive(ref groupEP);
                    string message = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    logger.Trace("Received broadcast from " + groupEP.ToString() + ". Message: " + message);
                }

            }
            catch (Exception e)
            {
                logger.Debug(e.ToString());
            }
        }
    }
}
