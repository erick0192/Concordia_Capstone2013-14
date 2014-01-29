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

namespace MarsRover
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
            listener = new UDPListener(port, this.MessageReceivedHandler);
        }     

        private void MessageReceivedHandler(int NumberOfAvailableData)
        {
            byte[] data = new byte[NumberOfAvailableData];

            listener.ReceiveData(ref data, NumberOfAvailableData);

            string message = Encoding.ASCII.GetString(data, 0, data.Length);
            messageQueue.Enqueue(message);
        }

        #endregion
    }
}
