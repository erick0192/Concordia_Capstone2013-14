using MarsRover;
using System.ComponentModel;

namespace RoverOperator.Content
{
    public class IMUViewModel : INotifyPropertyChanged
    {
        #region Properties

        public IMU IMUSensor { get; protected set; }

        #endregion

        #region Constructor

        public IMUViewModel()
        {
            IMUSensor = StatusUpdater.Instance.RoverStatus.IMUSensor;
            StatusUpdater.Instance.IMUUpdated += new StatusUpdater.IMUUpdatedDelegate(this.IMUSensorUpdatedHandler);
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public void IMUSensorUpdatedHandler(IMU imuSensor)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("IMUSensor"));
            }
        }

        #endregion
    }
}
