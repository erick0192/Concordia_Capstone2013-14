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
        // private WatchDogCore wd;
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public MessageListener(int port, IQueue messageQueue,
            String sourceIPAddress = "")
        {
            this.port = port;
            ipAddress = sourceIPAddress;
            this.messageQueue = messageQueue;
            //this.wd = wd;
        }

        #endregion

        #region Methods

        public void StartListening()
        {            
            listener = new UDPListener(port, this.MessageReceivedHandler);
        }     

        protected virtual void MessageReceivedHandler(int NumberOfAvailableData)
        {
            byte[] data = new byte[NumberOfAvailableData];

            listener.ReceiveData(ref data, NumberOfAvailableData);

            string message = Encoding.ASCII.GetString(data, 0, data.Length);
            messageQueue.Enqueue(message);
        }

        #endregion
    }

    public class MessageSender
    {
        private UdpClient sender = new UdpClient();
        private IQueue messageQueue;
        private int port;
        private string ipAddress;
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public MessageSender(int port, IQueue MessageQueue, string destinationIPAddress = "")
        {
            this.port = port;
            ipAddress = destinationIPAddress;
            messageQueue = MessageQueue;
            try
            {
                this.sender.Connect(ipAddress, port);
            }
            catch (Exception e)
            {
                logger.Error("{0}  " + e.Message, DateTime.Now.ToString());
            }
        }

        //send
        public void Send(string message)
        {
            try 
            {
                Byte[] bytesToSend = Encoding.ASCII.GetBytes(message);
                sender.Send(bytesToSend, bytesToSend.Length);
            }
            catch (Exception e)
            {
                logger.Error("{0}  " + e.Message, DateTime.Now.ToString());
            }
        }

        public void Send(Byte[] bytesToSend)
        {
            sender.Send(bytesToSend, bytesToSend.Length);
        }
        
    }
}
