using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover;

namespace MarsRoverClient.Content
{
    public class MotorStatusViewModel: INotifyPropertyChanged
    {
        private String motorKey;
        
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

        public MotorStatusViewModel(String motorKeyString)
        {
            title = "";
            motorKey = motorKeyString;
            motor = new Motor();
            StatusUpdater.Instance.MotorsStatusUpdated += new StatusUpdater.MotorsStatusUpdatedEventHandler(UpdateMotor);
        }

        #endregion

        #region Event Handlers

        private void UpdateMotor(Dictionary<String, Motor> motors)
        {
            Motor m = motors[motorKey];
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
