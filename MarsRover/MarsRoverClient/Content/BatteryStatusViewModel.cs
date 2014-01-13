using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover;

namespace MarsRoverClient.Content
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
            StatusUpdater.Instance.RoverStatusUpdated += new StatusUpdater.RoverStatusUpdatedEventHandler(UpdateBattery);
        }

        #endregion

        #region Event Handlers

        private void UpdateBattery(RoverStatus roverStatus)
        {
            this.battery.CurrentCharge = roverStatus.Battery.CurrentCharge;
            this.battery.Temperature = roverStatus.Battery.Temperature;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Battery"));
            }
        }

        #endregion
    }
}
