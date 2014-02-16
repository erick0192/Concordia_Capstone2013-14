using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using FirstFloor.ModernUI.Windows.Controls;
using RoverOperator.Content;
using NLog;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading;

namespace RoverOperator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties    
    
        private string connectedToRover;
        public string ConnectedToRover
        {
            get { return connectedToRover; }
            set
            {
                if (!value.Equals(connectedToRover))
                {
                    connectedToRover = value;
                    OnPropertyChanged("ConnectedToRover");
                }
            }
        }

        private string pingRTT;
        public string PingRTT
        {
            get { return pingRTT; }
            set
            {
                if (!value.Equals(pingRTT))
                {
                    pingRTT = value;
                    OnPropertyChanged("PingRTT");
                }
            }
        }     

        #endregion

        #region Delegates

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            ConnectedToRover = "Attempting to connect...";
            PingRTT = "Ping: 0 ms";
            StartPinging(Properties.NetworkSettings.Default.RoverIPAddress);
        }

        #endregion

        #region Event Handlers

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region Methods

        private void StartPinging(string host)
        {
            Thread t = new Thread(() => Ping(host));
            t.IsBackground = true;
            t.Start();
        }

        private void Ping(string host)
        {
            bool pingable = false;
            Ping pinger = new Ping();

            try
            {
                PingReply reply = pinger.Send(host);
                pingable = reply.Status == IPStatus.Success;
                PingRTT = "Ping: " + reply.RoundtripTime + " ms";
            }
            catch (PingException) { }

            if (pingable)
            {
                ConnectedToRover = "Connected";
            }
            else
            {
                ConnectedToRover = "Unable to connect";
            }
            Thread.Sleep(200);
        }

        #endregion

    }
}
