using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using MarsRoverClient.Log;

namespace MarsRoverClient.Content
{
    public class LogListViewModel : INotifyPropertyChanged
    {
        #region Properties

        public ObservableCollection<LogEntry> LogEntryList { get; protected set; }
        public ICollectionView LogEntryFilteredList
        {
            get;
            set;
        }

        private bool mCriticalFilterOn = true;                       
        public bool CriticalFilterOn {
            get { return mCriticalFilterOn; }
            protected set
            {
                mCriticalFilterOn = value;
                OnPropertyChanged("CriticalFilterOn");
                LogEntryFilteredList.Refresh();
            }
        }

        private bool mEssentialFilterOn = true;
        public bool EssentialFilterOn
        {
            get { return mEssentialFilterOn; }
            protected set
            {
                mEssentialFilterOn = value;
                OnPropertyChanged("EssentialFilterOn");
                LogEntryFilteredList.Refresh();
            }
        }

        private bool mInfoFilterOn = true;
        public bool InfoFilterOn
        {
            get { return mInfoFilterOn; }
            protected set
            {
                mInfoFilterOn = value;
                OnPropertyChanged("InfoFilterOn");
                LogEntryFilteredList.Refresh();
            }
        }

        private bool mDebugFilterOn = false;
        public bool DebugFilterOn
        {
            get { return mDebugFilterOn; }
            protected set
            {
                mDebugFilterOn = value;
                OnPropertyChanged("DebugFilterOn");
                LogEntryFilteredList.Refresh();
            }
        }

        #endregion

        #region Commands

        private ICommand mToggleCriticalFilterCommand;
        public ICommand ToggleCriticalFilterCommand
        {
            get
            {
                if (mToggleCriticalFilterCommand == null)
                {
                    mToggleCriticalFilterCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ToggleCriticalFilter(p),
                        p => this.CanToggleFilters());
                }
                return mToggleCriticalFilterCommand;
            }
        }

        private ICommand mToggleEssentialFilterCommand;
        public ICommand ToggleEssentialFilterCommand
        {
            get
            {
                if (mToggleEssentialFilterCommand == null)
                {
                    mToggleEssentialFilterCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ToggleEssentialFilter(p),
                        p => this.CanToggleFilters());
                }
                return mToggleEssentialFilterCommand;
            }
        }

        private ICommand mToggleInfoFilterCommand;
        public ICommand ToggleInfoFilterCommand
        {
            get
            {
                if (mToggleInfoFilterCommand == null)
                {
                    mToggleInfoFilterCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ToggleInfoFilter(p),
                        p => this.CanToggleFilters());
                }
                return mToggleInfoFilterCommand;
            }
        }

        private ICommand mToggleDebugFilterCommand;
        public ICommand ToggleDebugFilterCommand
        {
            get
            {
                if (mToggleDebugFilterCommand == null)
                {
                    mToggleDebugFilterCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ToggleDebugFilter(p),
                        p => this.CanToggleFilters());
                }
                return mToggleDebugFilterCommand;
            }
        }

        private ICommand mClearLogCommand;
        public ICommand ClearLogCommand
        {
            get
            {
                if (mClearLogCommand == null)
                {
                    mClearLogCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ClearLog(),
                        p => this.CanClearLog());
                }
                return mClearLogCommand;
            }
        }


        #endregion        
       
        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public LogListViewModel()
        {
            LogEntryList = ApplicationLogger.Instance.LogEntryList;
            ApplicationLogger.Instance.LogEntryEvent += new ApplicationLogger.LogEntryHandler(LogEntryAdded);
            ApplicationLogger.Instance.LogClearedEvent += new ApplicationLogger.LogClearedHandler(LogCleared);

            var icv = CollectionViewSource.GetDefaultView(LogEntryList);
            icv.Filter = LogEntryFilter;
            LogEntryFilteredList = icv;
        }

        private bool LogEntryFilter(object item)
        {
            
            if (null == item)
                return false;

            LogEntry le = item as LogEntry;
            bool filter =
                (le.Level == LogLevel.Critical && CriticalFilterOn) ||
                (le.Level == LogLevel.Essential && EssentialFilterOn) ||
                (le.Level == LogLevel.Info && InfoFilterOn) ||
                (le.Level == LogLevel.Debug && DebugFilterOn);

            return filter;
        }

        #endregion

        #region Command Methods      

        protected bool CanToggleFilters() { return true; }

        protected void ToggleCriticalFilter(object iParam)
        {
            CriticalFilterOn = (bool)iParam;
            ApplicationLogger.Instance.Log(String.Format("Critical Filter has been turned {0}.", (bool)iParam ? "on" : "off"), LogLevel.Debug);
        }

        protected void ToggleEssentialFilter(object iParam)
        {
            EssentialFilterOn = (bool)iParam;
            ApplicationLogger.Instance.Log(String.Format("Essential Filter has been turned {0}.", (bool)iParam ? "on" : "off"), LogLevel.Debug);
        }

        protected void ToggleInfoFilter(object iParam)
        {
            InfoFilterOn = (bool)iParam;
            ApplicationLogger.Instance.Log(String.Format("Info Filter has been turned {0}.", (bool)iParam ? "on" : "off"), LogLevel.Debug);
        }

        protected void ToggleDebugFilter(object iParam)
        {
            DebugFilterOn = (bool)iParam;
            ApplicationLogger.Instance.Log(String.Format("Debug Filter has been turned {0}.", (bool)iParam ? "on" : "off"), LogLevel.Debug);
        }

        protected bool CanClearLog() { return true; }
        protected void ClearLog()
        {
            ApplicationLogger.Instance.ClearLog();
            ApplicationLogger.Instance.Log("The log has been cleared.", LogLevel.Essential);
           
        }

        #endregion

        #region Event Handlers

        public void LogEntryAdded(LogEntry iLogEntry)
        {
            //Note: LogList in this class is the same object as the one in ApplicationLogger
            OnPropertyChanged("LogList");
        }

        public void LogCleared()
        {            
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
