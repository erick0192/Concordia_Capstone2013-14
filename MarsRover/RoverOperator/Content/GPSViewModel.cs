using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
