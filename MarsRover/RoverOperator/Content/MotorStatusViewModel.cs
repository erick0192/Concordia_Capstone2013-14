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

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public MotorStatusViewModel(Motor.Location motorKey)
        {
            this.motorKey = motorKey;
            motor = StatusUpdater.Instance.RoverStatus.Motors[motorKey];
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

        #endregion
    }     
}
