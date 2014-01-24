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
    public class CommandsListener
    {
        #region Private Attributes

        private static CommandsListener instance;
        private UDPListener listener;
        private IQueue commanderDispatcherMessageQueue;
        private volatile bool listen;
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();

        #endregion

        #region Properties

        public static CommandsListener Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommandsListener();
                }

                return instance;
            }
        }

        #endregion

        #region Constructor

        private CommandsListener()
        {
            
        }

        #endregion

        #region Methods

        public void StartListening(int port, IQueue messageQueue, String sourceIPAddress = "")
        {
            commanderDispatcherMessageQueue = messageQueue;
            listener = new UDPListener(port, sourceIPAddress == "" ? IPAddress.Any : IPAddress.Parse(sourceIPAddress));
            listener.Initialize();

            Thread t = new Thread(CheckQueue) { IsBackground = true };
            listen = true;
            t.Start();

        }        

        private void CheckQueue()
        {
            while(listen)
            {
                IEnumerable<string> messages = listener.MessagesQueue;
                foreach(string m in messages)
                {
                    logger.Debug(m);
                    commanderDispatcherMessageQueue.Enqueue(m);
                }
            }
        }

        public void Stop()
        {
            listen = false;
            try
            {
                //we should close the port here.
            }
            catch(SocketException se)
            {
                
            }
        }

        #endregion
    }
}
