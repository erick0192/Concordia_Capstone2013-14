using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace MarsRover.Communication
{
    public class MessageListener
    {
        #region Private Attributes

        private UDPListener listener;
        private IQueue messageQueue;
        private int port;
        private string ipAddress;
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public MessageListener(int port, IQueue messageQueue, String sourceIPAddress = "")
        {
            this.port = port;
            ipAddress = sourceIPAddress;
            this.messageQueue = messageQueue;
        }

        #endregion

        #region Methods

        public void StartListening()
        {            
            listener = new UDPListener(port, this.ipAddress == "" ? IPAddress.Any : IPAddress.Parse(this.ipAddress));
            listener.Initialize();

            Thread t = new Thread(CheckQueue) { IsBackground = true };
            t.Start();
        }     

        private void CheckQueue()
        {
            while(true)
            {
                IEnumerable<string> messages = listener.MessagesQueue;
                foreach(string m in messages)
                {
                    //logger.Debug(m);
                    messageQueue.Enqueue(m);
                }
            }
        }

        #endregion
    }
}
