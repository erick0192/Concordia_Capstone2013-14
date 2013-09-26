using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRClient_ModernUIProtoss.Log;

namespace MRClient_ModernUIProtoss.Content
{
    public class LogListViewModel : INotifyPropertyChanged
    {
        #region Properties

        public ObservableCollection<LogEntry> LogList { get; protected set; }       

        #endregion

        #region Commands

        //private ICommand mToggleCamera;
        //public ICommand ToggleCamera
        //{
        //    get
        //    {
        //        if (mToggleCamera == null)
        //        {
        //            mToggleCamera = new FirstFloor.ModernUI.Presentation.RelayCommand(
        //                p => this.ToggleCam(),
        //                p => this.CanToggleCamera());
        //        }
        //        return mToggleCamera;
        //    }
        //}

        #endregion        
       
        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public LogListViewModel()
        {
            LogList = ApplicationLogger.Instance.LogList; 
            ApplicationLogger.Instance.LogEntryEvent += new ApplicationLogger.LogEntryHandler(AddLogEntry);                       
        }

        #endregion

        #region Command Methods      

        

        #endregion

        #region Event Handlers

        public void AddLogEntry(LogEntry iLogEntry)
        {
            //Note: LogList in this class is the same object as the one in ApplicationLogger
            OnPropertyChanged("LogList");
        }

        protected void OnPropertyChanged(string iPropertyName)
        {            
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(iPropertyName));
            }
        }

        #endregion
    }
}
