﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RoverOperator.Content
{
    public class NetworkSettingsViewModel : INotifyPropertyChanged
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

        #endregion
    

    }
}
