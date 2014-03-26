using System.ComponentModel;
using MarsRover;

namespace RoverOperator.Content
{
    public class GPSViewModel : INotifyPropertyChanged
    {
        #region Properties

        public GPSCoordinates Coordinates { get; protected set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public GPSViewModel()
        {
            Coordinates = StatusUpdater.Instance.RoverStatus.GPSCoordinates;
            StatusUpdater.Instance.GPSCoordinatesUpdated += new StatusUpdater.GPSCoordinatesUpdatedDelegate(this.GPSUpdatedHandler);
        }

        #endregion

        #region Methods

        public void GPSUpdatedHandler(GPSCoordinates coordinates)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Coordinates"));
            }
        }

        #endregion
    }
}
