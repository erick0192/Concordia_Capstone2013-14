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

        private string roverIPAddress;
        public string RoverIPAddress
        {
            get
            {
                return Properties.Settings.Default.RoverIPAddress;
            }
            set
            {
                Properties.Settings.Default.RoverIPAddress = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RoverIPAddress"));
                }
            }
        }

        private int cameraPort1;
        public int CameraPort1
        {
            get
            {
                return Properties.Settings.Default.CameraPort1;
            }
            set
            {
                Properties.Settings.Default.CameraPort1 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort1"));
                }
            }
        }

        private int cameraPort2;
        public int CameraPort2
        {
            get
            {
                return Properties.Settings.Default.CameraPort2;
            }
            set
            {
                Properties.Settings.Default.CameraPort2 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort2"));
                }
            }
        }

        private int cameraPort3;
        public int CameraPort3
        {
            get
            {
                return Properties.Settings.Default.CameraPort3;
            }
            set
            {
                Properties.Settings.Default.CameraPort3 = value;
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
            Properties.Settings.Default.Save();
        }
    }
}
