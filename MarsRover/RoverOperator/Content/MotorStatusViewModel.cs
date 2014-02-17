using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public float Duty
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
                    PropertyChanged(this, new PropertyChangedEventArgs("Duty"));
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
            
            motor.DangerousCurrentDetected += new MarsRover.Motor.WarningCurrentDetectedDelegate(CurrentStatusChanged);
            motor.WarningCurrentDetected += new MarsRover.Motor.WarningCurrentDetectedDelegate(CurrentStatusChanged);
            motor.NormalCurrentDetected += new MarsRover.Motor.NormalCurrentDetectedDelegate(CurrentStatusChanged);

            motor.DangerousTemperatureDetected += new MarsRover.Motor.DangerousTemperatureDetectedDelegate(TemperatureStatusChanged);
            motor.WarningTemperatureDetected += new MarsRover.Motor.WarningTemperatureDetectedDelegate(TemperatureStatusChanged);
            motor.NormalTemperatureDetected += new MarsRover.Motor.NormalTemperatureDetectedDelegate(TemperatureStatusChanged);

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
            if (motor.StatusTemperature == Motor.TemperatureStatus.Dangerous)
            {
                IsDangerousTemperature = true;
                IsWarningTemperature = false;
            }
            else if(motor.StatusTemperature == Motor.TemperatureStatus.Warning)
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
            if(motor.StatusCurrent == Motor.CurrentStatus.Dangerous)
            {
                IsDangerousCurrent = true;
                IsWarningCurrent = false;
            }
            else if (motor.StatusCurrent == Motor.CurrentStatus.Warning)
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
