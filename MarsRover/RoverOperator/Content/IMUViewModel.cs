using MarsRover;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
