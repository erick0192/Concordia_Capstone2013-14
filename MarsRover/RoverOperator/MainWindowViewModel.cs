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

        #region Events/Delegates

        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void RTTEventHandler(long RTT);

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            ConnectedToRover = "Attempting to connect...";
            PingRTT = "Ping: 0 ms";
            MarsRover.Communication.Pinger.Instance.RTTChanged += RTTChanged;
            MarsRover.Communication.Pinger.Instance.ConnectivityChanged += ConnectivityChanged;
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

        private void RTTChanged(long RTT)
        {
            PingRTT = "Ping: " + RTT + " ms";
        }

        private void ConnectivityChanged(bool connectedToRover)
        {
            if (connectedToRover)
            {
                ConnectedToRover = "Connected";
            }
            else
            {
                ConnectedToRover = "Unable to connect...";
            }
        }

        #endregion

    }
}
