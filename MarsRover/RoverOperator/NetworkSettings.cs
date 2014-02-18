//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RoverOperator
//{
//    public class NetworkSettings : INotifyPropertyChanged
//    {

//        #region Properties

//        public string RoverIPAddress
//        {
//            get
//            {
//                return Properties.NetworkSettings.Default.RoverIPAddress;
//            }
//            set
//            {
//                Properties.NetworkSettings.Default.RoverIPAddress = value;
//                MarsRover.Communication.Pinger.Instance.RoverIPAddress = value;
//                if (PropertyChanged != null)
//                {
//                    PropertyChanged(this, new PropertyChangedEventArgs("RoverIPAddress"));
//                }
//            }
//        }

//        public int CameraPort1
//        {
//            get
//            {
//                return Properties.NetworkSettings.Default.CameraPort1;
//            }
//            set
//            {
//                Properties.NetworkSettings.Default.CameraPort1 = value;
//                if (PropertyChanged != null)
//                {
//                    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort1"));
//                }
//            }
//        }

//        public int CameraPort2
//        {
//            get
//            {
//                return Properties.NetworkSettings.Default.CameraPort2;
//            }
//            set
//            {
//                Properties.NetworkSettings.Default.CameraPort2 = value;
//                if (PropertyChanged != null)
//                {
//                    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort2"));
//                }
//            }
//        }

//        public int CameraPort3
//        {
//            get
//            {
//                return Properties.NetworkSettings.Default.CameraPort3;
//            }
//            set
//            {
//                Properties.NetworkSettings.Default.CameraPort3 = value;
//                if (PropertyChanged != null)
//                {
//                    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort3"));
//                }
//            }
//        }

//        public int CommandsPort
//        {
//            get
//            {
//                return Properties.NetworkSettings.Default.CommandsPort;
//            }
//            set
//            {
//                Properties.NetworkSettings.Default.CameraPort3 = value;
//                if (PropertyChanged != null)
//                {
//                    PropertyChanged(this, new PropertyChangedEventArgs("CommandsPort"));
//                }
//            }
//        }

//        private static NetworkSettings instance;
//        public static NetworkSettings Instance
//        {
//            get
//            {
//                if(instance == null)
//                {
//                    instance = new NetworkSettings();
//                }
//                return instance;
//            }
//        }

//        #endregion

//        #region Delegates and Events

//        public event PropertyChangedEventHandler PropertyChanged;
        
//        #endregion

//        private NetworkSettings()
//        {
//            RoverIPAddress = Properties.NetworkSettings.Default.RoverIPAddress;
//        }

//        public void Save()
//        {
//            Properties.NetworkSettings.Default.Save();
//        }

//        public void ResetToDefault()
//        {
//            Properties.NetworkSettings.Default.Reset();
//        }
//    }
//}
