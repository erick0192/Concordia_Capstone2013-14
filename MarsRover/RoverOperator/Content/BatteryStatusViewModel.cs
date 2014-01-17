﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover;

namespace RoverOperator.Content
{
    public class BatteryStatusViewModel : INotifyPropertyChanged
    {
        #region Properties

        private Battery battery;
        public Battery Battery
        {
            get
            {
                return battery;
            }
        }

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public BatteryStatusViewModel()
        {
            battery = (Battery)StatusUpdater.Instance.RoverStatus.Battery.Clone();
            StatusUpdater.Instance.BatteryUpdated += new StatusUpdater.BatteryUpdatedDelegate(UpdateBattery);
        }

        #endregion

        #region Event Handlers

        private void UpdateBattery(Battery battery)
        {
            this.battery.CurrentCharge = battery.CurrentCharge;
            this.battery.Temperature = battery.Temperature;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Battery"));
            }
        }

        #endregion
    }
}
