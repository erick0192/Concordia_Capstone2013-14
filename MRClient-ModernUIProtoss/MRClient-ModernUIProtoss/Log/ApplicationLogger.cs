using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRClient_ModernUIProtoss.Log
{
    public class ApplicationLogger
    {
        #region Private Members
        
        private static ApplicationLogger mInstance;
        private Object mLogLock = new Object();
        private ObservableCollection<LogEntry> mLogEntryList  = new ObservableCollection<LogEntry>();

        #endregion

        #region Properties

        public LogLevel LogLevel { get; set; }
        public bool LogToDebugConsole { get; set; }
        public ObservableCollection<LogEntry> LogEntryList
        {
            get { return mLogEntryList; }
            protected set
            {
                lock (mLogLock)
                {
                    mLogEntryList = value;
                }
            }
        }

        public static ApplicationLogger Instance
        {
            get 
            {
                if (mInstance == null)
                 {
                    mInstance = new ApplicationLogger();
                 }
                 return mInstance;
            }
        }

        #endregion

        #region Delegates and Events

        public delegate void LogEntryHandler(LogEntry iLogEntry);
        public event LogEntryHandler LogEntryEvent;
        public delegate void LogClearedHandler();
        public event LogClearedHandler LogClearedEvent;

        #endregion

        #region Constructors

        private ApplicationLogger()
        {
            LogLevel = LogLevel.Essential;
            LogToDebugConsole = false;
        }

        #endregion

        #region Methods

        public LogEntry Log(string iMessage, LogLevel iLevel)
        {
            LogEntry le = null;

            if (iLevel <= LogLevel)
            {
                le = new LogEntry(iMessage, DateTime.Now, iLevel);
                
                lock (mLogLock)
                {
                    LogEntryList.Insert(0, le); //Need to monitor performance of this. Might need to implement our own observable stack

                }

                if (LogEntryEvent != null)
                {
                    //Fire event to notify a log entry has been made
                    LogEntryEvent(le);
                }

                if(LogToDebugConsole)
                {
                    System.Diagnostics.Debug.WriteLine(le.ToString());
                }
            }

            return le;
        }

        public void ClearLog()
        {
            lock (mLogLock)
            {
                LogEntryList.Clear();
            }

            if (LogClearedEvent != null)
            {
                //Fire event to notify that the log has been cleared
                LogClearedEvent();
            }
        }

        #endregion
    }
}
