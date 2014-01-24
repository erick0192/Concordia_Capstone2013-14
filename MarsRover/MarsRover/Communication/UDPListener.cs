using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NLog;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MarsRover.Communication
{
    public class UDPListener
    {
        #region Attributes
        private int port;
        private IPAddress ipAddress;
        private Logger logger;

        private UdpClient listener;
        private IPEndPoint groupEP;
        private BlockingCollection<string> messagesQueue;
        #endregion

        #region Properties
        public IEnumerable<string> MessagesQueue
        {
            get { return messagesQueue.GetConsumingEnumerable(); }
        }
        #endregion

        public UDPListener(int port) : this (port, IPAddress.Any)
        {
            
        }

        public UDPListener(int port, IPAddress ipAddress)
        {
            this.port = port;
            this.ipAddress = ipAddress;
            logger = NLog.LogManager.GetCurrentClassLogger();
            messagesQueue = new BlockingCollection<string>(); //uses ConcurrentQueue by default
        }

        public void Initialize()
        {
            Thread t = new Thread(StartListening);
            t.IsBackground = true;
            t.Start();
        }


        private void StartListening()
        {
            try
            {
                listener = new UdpClient(port);
                groupEP = new IPEndPoint(ipAddress, port);

                while (true)
                {
                    logger.Trace("Starting UDPListener on port " + port);
                    byte[] bytes = listener.Receive(ref groupEP);
                    string message = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    logger.Trace("Received broadcast from " + groupEP.ToString() + ". Message: " + message);
                    messagesQueue.Add(message);
                }

            }
            catch (Exception e)
            {
                logger.Debug(e.ToString());
            }
        }
    }
}
