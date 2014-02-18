using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NLog;

namespace MarsRover.Communication
{
    public sealed class Pinger
    {
        #region Properties
        private string roverIPAddress;
        public string RoverIPAddress
        {
            get
            {
                return roverIPAddress;
            }
            set
            {
                roverIPAddress = value;
            }
        }

        private long RTT;
        public long PingRTT
        {
            get { return RTT; }
            set
            {
                if (RTTChanged != null) RTTChanged(value);
                RTT = value;
            }
        }

        private bool connectedToRover;
        public bool ConnectedToRover
        {
            get { return connectedToRover; }
            set
            {
                if (ConnectivityChanged != null) ConnectivityChanged(value);
                connectedToRover = value;
            }
        }

        #endregion

        #region Event/Delegates

        public delegate void OnRTTChanged(long RTT);
        public event OnRTTChanged RTTChanged;

        public delegate void OnConnectivityChanged(bool connectedToRover);
        public event OnConnectivityChanged ConnectivityChanged;

        #endregion

        #region Singleton Constructor/Properties

        //Critical area of the constructor is locked so that only one instance is created even in a multithreaded environment
        //http://msdn.microsoft.com/en-us/library/ff650316.aspx
        private static volatile Pinger instance;
        private static object syncRoot = new Object();

        private Pinger()
        {
            StartPinging();
        }

        public static Pinger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Pinger();
                        }
                    }
                }

                return instance;
            }
        }

        private void StartPinging()
        {
            Thread t = new Thread(() => Ping());
            t.IsBackground = true;
            t.Start();
        }

        private void Ping()
        {
            bool pingable = false;
            Ping pinger = new Ping();
            while (true)
            {
                try
                {
                    PingReply reply = pinger.Send(roverIPAddress);
                    pingable = reply.Status == IPStatus.Success;
                    PingRTT = reply.RoundtripTime;
                }
                catch (PingException) { }

                if (pingable)
                {
                    ConnectedToRover = true;
                }
                else
                {
                    ConnectedToRover = false;
                }
                Thread.Sleep(200);
            }
        }

        #endregion



    }
}
