using System.ComponentModel;
using MarsRover;

namespace RoverOperator.Content
{
    public class RoboticArmViewModel : INotifyPropertyChanged
    {
        #region Properties

        private RoboticArm roboticArm;
        public RoboticArm RoboticArm
        {
            get { return roboticArm; }
        }

        #endregion

        #region Delegates and Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor

        public RoboticArmViewModel()
        {
            roboticArm = StatusUpdater.Instance.RoverStatus.RoboArm;
            StatusUpdater.Instance.RoboticArmUpdated += new StatusUpdater.RoboticArmUpdatedDelegate(UpdateRoboticArm);
        }

        #endregion

        #region Event Handlers

        private void UpdateRoboticArm(RoboticArm ra)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("RoboticArm"));
            }
        }

        #endregion
    }
}
