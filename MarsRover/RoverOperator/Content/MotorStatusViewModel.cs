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

        private String title;
        public String Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                }
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

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public MotorStatusViewModel(Motor.Location motorKey)
        {
            title = "";
            this.motorKey = motorKey;
            motor = new Motor();
            StatusUpdater.Instance.RoverStatusUpdated += new StatusUpdater.RoverStatusUpdatedEventHandler(UpdateMotor);
        }

        #endregion

        #region Event Handlers

        private void UpdateMotor(RoverStatus roverStatus)
        {
            Motor m = roverStatus.Motors[motorKey];
            motor.Current = m.Current;
            motor.Temperature = m.Temperature;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Motor"));
            }
        }

        #endregion
    }     
}
