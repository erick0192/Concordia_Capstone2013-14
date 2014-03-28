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

        public RoverOperator.Pages.MainViewModel MainVM { get; set; }

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

            Properties.NetworkSettings.Default.PropertyChanged += new PropertyChangedEventHandler(UpdatePingAddress);
            MarsRover.Communication.Pinger.Instance.RoverIPAddress = Properties.NetworkSettings.Default.RoverIPAddress;
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

        private void UpdatePingAddress(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RoverIPAddress")
            {
                MarsRover.Communication.Pinger.Instance.RoverIPAddress = Properties.NetworkSettings.Default.RoverIPAddress;
            }
        }

        #endregion


       
    }
}
