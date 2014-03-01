using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Threading;

namespace RoverOperator.Content
{
    public class NetworkSettingsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Properties

        private string validRoverIPAddress;
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
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs("RoverIPAddress"));
                //}
            }
        }

        private int cameraPort1;
        public int CameraPort1
        {
            get
            {
                return cameraPort1;
            }
            set
            {
                cameraPort1 = value;
                //if(PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort1"));
                //}
            }
        }

        private int cameraPort2;
        public int CameraPort2
        {
            get
            {
                return cameraPort2;
            }
            set
            {
                cameraPort2 = value;
                //if(PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort2"));
                //}
            }
        }

        private int cameraPort3;
        public int CameraPort3
        {
            get
            {
                return cameraPort3;
            }
            set
            {
                cameraPort3 = value;
                //if(PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs("CameraPort3"));
                //}
            }
        }

        //Validation for Network Properties
        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string property]
        {
            get
            {
                string result = null;
                
                if (property == "RoverIPAddress") {
                    if (!IPAddressIsValid(roverIPAddress))
                    {
                        result = "Invalid IP Address";
                    }
                }

                return result;
            }
        }

        #endregion

        #region Commands

        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(p => this.Save());
                }
                return saveCommand;
            }
        }

        private ICommand resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                if(resetCommand == null)
                {
                    resetCommand = new RelayCommand(p => this.Reset());
                }
                return resetCommand;
            }           
        }

        private ICommand defaultCommand;
        public ICommand DefaultCommand
        {
            get
            {
                if(defaultCommand == null)
                {
                    defaultCommand = new RelayCommand(p => this.Default());
                }
                return defaultCommand;
            }
        }

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public NetworkSettingsViewModel()
        {
            Reset();
            Thread t = new Thread(() => logstuff());
            t.IsBackground = true;
            t.Start();
        }

        private void logstuff()
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            while (true)
            {
                //logger.Debug(roverIPAddress);
                Thread.Sleep(100);
            }
        }

        #endregion

        #region Command Methods

        private void Save()
        {
            Properties.NetworkSettings.Default.RoverIPAddress = roverIPAddress;
            Properties.NetworkSettings.Default.CameraPort1 = cameraPort1;
            Properties.NetworkSettings.Default.CameraPort2 = cameraPort2;
            Properties.NetworkSettings.Default.CameraPort3 = cameraPort3;

            Properties.NetworkSettings.Default.Save();
        }

        private void Reset()
        {
            roverIPAddress = Properties.NetworkSettings.Default.RoverIPAddress;
            cameraPort1 = Properties.NetworkSettings.Default.CameraPort1;
            cameraPort2 = Properties.NetworkSettings.Default.CameraPort2;
            cameraPort3 = Properties.NetworkSettings.Default.CameraPort3;

            FirePropertiesChanged();
        }

        private void Default()
        {
            Properties.NetworkSettings.Default.Reset();
            Properties.NetworkSettings.Default.Save();
            Reset();
        }

        private void FirePropertiesChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("RoverIPAddress"));
                PropertyChanged(this, new PropertyChangedEventArgs("CameraPort1"));
                PropertyChanged(this, new PropertyChangedEventArgs("CameraPort2"));
                PropertyChanged(this, new PropertyChangedEventArgs("CameraPort3"));
            }
        }

        private bool IPAddressIsValid(string IPAddress)
        {
            Match match = Regex.Match(roverIPAddress, @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
            if (match.Success) return true;
            return false;
        }

        #endregion
    

    }
}
