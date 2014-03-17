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
    public class GuardedMessageListener : MessageListener
    {
        private WatchDogCore _wd;

        public GuardedMessageListener(int port, IQueue messageQueue,
            String sourceIPAddress, WatchDogCore wd = null) :
            base(port, messageQueue, sourceIPAddress)
            {
                _wd = wd;
            }

        protected override void MessageReceivedHandler(int NumberOfAvailableData)
        {
            base.MessageReceivedHandler(NumberOfAvailableData);
            _wd.reportActivity();
        }
    }
}
