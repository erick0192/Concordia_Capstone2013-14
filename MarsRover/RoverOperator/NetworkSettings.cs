using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverOperator
{
    public class NetworkSettings : INotifyPropertyChanged
    {

        #region Properties

        //private string roverIPAddress;
        public string RoverIPAddress
        {
            get
            {
                return Properties.NetworkSettings.Default.RoverIPAddress;
            }
            set
            {
                Properties.NetworkSettings.Default.RoverIPAddress = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RoverIPAddress"));
                }
            }
        }

        //private int cameraPort1;
        public int CameraPort1
        {
            get
            {
                return Properties.NetworkSettings.Default.CameraPort1;
            }
            set
            {
                Properties.NetworkSettings.Default.CameraPort1 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort1"));
                }
            }
        }

        //private int cameraPort2;
        public int CameraPort2
        {
            get
            {
                return Properties.NetworkSettings.Default.CameraPort2;
            }
            set
            {
                Properties.NetworkSettings.Default.CameraPort2 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort2"));
                }
            }
        }

        //private int cameraPort3;
        public int CameraPort3
        {
            get
            {
                return Properties.NetworkSettings.Default.CameraPort3;
            }
            set
            {
                Properties.NetworkSettings.Default.CameraPort3 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort3"));
                }
            }
        }

        private static NetworkSettings instance;
        public static NetworkSettings Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new NetworkSettings();
                }
                return instance;
            }
        }

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;
        
        #endregion

        private NetworkSettings()
        {
            
        }

        public void Save()
        {
            Properties.NetworkSettings.Default.Save();
        }

        public void ResetToDefault()
        {
            Properties.NetworkSettings.Default.Reset();
        }
    }
}
