using System;
using System.ComponentModel;
using MarsRover;

namespace RoverOperator.Content
{
    public class MotorStatusViewModel: INotifyPropertyChanged
    {
        private Motor.Location motorKey;
        
        #region Properties

        public String Title
        {
            get
            {
                return MarsRover.Motor.GetLocationFriendlyString(this.motorKey);
            }
           
        }

        private Motor motor;
        public Motor Motor
        {
            get
            {
                return motor;
            }
        }

        public float Power
        {
            get
            {
                return Motor.Duty;
            }
            set
            {
                Motor.Duty = (float)Math.Round(value);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Power"));
                }
            }
        }

        private bool canModifyPower = false;
        public bool CanModifyPower
        {
            get
            {
                return canModifyPower;
            }
            set
            {
                canModifyPower = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CanModifyPower"));
                }
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

        public MotorStatusViewModel(Motor.Location motorKey)
        {
            this.motorKey = motorKey;
            motor = StatusUpdater.Instance.RoverStatus.Motors[motorKey];
            
            motor.DangerousCurrentDetected += new MarsRover.DangerousCurrentDetectedDelegate<Motor>(CurrentStatusChanged);
            motor.WarningCurrentDetected += new MarsRover.WarningCurrentDetectedDelegate<Motor>(CurrentStatusChanged);
            motor.NormalCurrentDetected += new MarsRover.NormalCurrentDetectedDelegate<Motor>(CurrentStatusChanged);

            motor.DangerousTemperatureDetected += new MarsRover.DangerousTemperatureDetectedDelegate<Motor>(TemperatureStatusChanged);
            motor.WarningTemperatureDetected += new MarsRover.WarningTemperatureDetectedDelegate<Motor>(TemperatureStatusChanged);
            motor.NormalTemperatureDetected += new MarsRover.NormalTemperatureDetectedDelegate<Motor>(TemperatureStatusChanged);

            StatusUpdater.Instance.MotorsUpdated += new StatusUpdater.MotorsUpdatedDelegate(UpdateMotor);
        }

        #endregion

        #region Event Handlers

        private void UpdateMotor(Motor motor)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Motor"));
            }
        }

        private void TemperatureStatusChanged(Motor motor)
        {
            if (motor.StatusTemperature == TemperatureStatus.Dangerous)
            {
                IsDangerousTemperature = true;
                IsWarningTemperature = false;
            }
            else if(motor.StatusTemperature == TemperatureStatus.Warning)
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

        private void CurrentStatusChanged(Motor motor)
        {
            if(motor.StatusCurrent == CurrentStatus.Dangerous)
            {
                IsDangerousCurrent = true;
                IsWarningCurrent = false;
            }
            else if (motor.StatusCurrent == CurrentStatus.Warning)
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
