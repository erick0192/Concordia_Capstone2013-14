using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverClient.Content
{
    public class GeneralSettingsViewModel : INotifyPropertyChanged
    {
        #region Properties

        public int BatteryStatusUpdateInterval
        {
            get { return StatusUpdater.Instance.BatteryUpdateInterval; }
        }

        public int MotorsStatusUpdateInterval
        {
            get { return StatusUpdater.Instance.MotorsUpdateInterval; }
        }

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public void Save()
        {
            
        }

        #endregion
    }
}
