using System.ComponentModel;
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

        public bool IsDangerousCurrent { get; protected set; }
        public bool IsWarningCurrent { get; protected set; }

        public bool IsDangerousTemperature { get; protected set; }
        public bool IsWarningTemperature { get; protected set; }  

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public BatteryStatusViewModel()
        {
            battery = StatusUpdater.Instance.RoverStatus.Battery;

            battery.DangerousCurrentDetected += new MarsRover.DangerousCurrentDetectedDelegate<Battery>(CurrentStatusChanged);
            battery.WarningCurrentDetected += new MarsRover.WarningCurrentDetectedDelegate<Battery>(CurrentStatusChanged);
            battery.NormalCurrentDetected += new MarsRover.NormalCurrentDetectedDelegate<Battery>(CurrentStatusChanged);

            battery.DangerousTemperatureDetected += new MarsRover.DangerousTemperatureDetectedDelegate<Battery>(TemperatureStatusChanged);
            battery.WarningTemperatureDetected += new MarsRover.WarningTemperatureDetectedDelegate<Battery>(TemperatureStatusChanged);
            battery.NormalTemperatureDetected += new MarsRover.NormalTemperatureDetectedDelegate<Battery>(TemperatureStatusChanged);

            StatusUpdater.Instance.BatteryUpdated += new StatusUpdater.BatteryUpdatedDelegate(UpdateBattery);
        }

        #endregion

        #region Event Handlers

        private void UpdateBattery(Battery battery)
        {            
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Battery"));
            }
        }

        private void TemperatureStatusChanged(Battery battery)
        {
            if (battery.StatusTemperature == TemperatureStatus.Dangerous)
            {
                IsDangerousTemperature = true;
                IsWarningTemperature = false;
            }
            else if (battery.StatusTemperature == TemperatureStatus.Warning)
            {
                IsDangerousTemperature = false;
                IsWarningTemperature = true;
            }
            else
            {
                IsDangerousTemperature = false;
                IsWarningTemperature = false;
            }

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("IsDangerousTemperature"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsWarningTemperature"));
            }
        }

        private void CurrentStatusChanged(Battery battery)
        {
            if (battery.StatusCurrent == CurrentStatus.Dangerous)
            {
                IsDangerousCurrent = true;
                IsWarningCurrent = false;
            }
            else if (battery.StatusCurrent == CurrentStatus.Warning)
            {
                IsDangerousCurrent = false;
                IsWarningCurrent = true;
            }
            else
            {
                IsDangerousCurrent = false;
                IsWarningCurrent = false;
            }

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("IsDangerousCurrent"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsWarningCurrent"));
            }
        }

        #endregion
    }

}
