using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRClient_ModernUIProtoss.Log
{
    public enum LogLevel
    {
        Critical = 0,
        Essential = 1,
        Information = 2,
        Debug = 3
    }

    public class ApplicationLogger
    {
        #region Private Members
        
        private static ApplicationLogger mInstance;

        #endregion

        #region Properties

        public LogLevel LogLevel { get; set; }
        public bool LogToSystemConsole { get; set; }

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

        #endregion

        #region Constructors

        private ApplicationLogger()
        {
            LogLevel = LogLevel.Essential;
            LogToSystemConsole = false;
        }

        #endregion

        #region Methods

        public LogEntry Log(string iMessage, LogLevel iLevel)
        {
            LogEntry le = null;

            if (iLevel <= LogLevel)
            {
                le = new LogEntry(iMessage, DateTime.Now, iLevel);
                if (LogEntryEvent != null)
                {
                    //Fire event to notify a log entry has been made
                    LogEntryEvent(le);
                }

                if (LogToSystemConsole)
                {
                    System.Console.WriteLine(le.ToString());
                }
            }

            return le;
        }

        #endregion
    }
}
