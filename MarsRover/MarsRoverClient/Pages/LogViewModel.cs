using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MarsRoverClient.Content;
using MarsRoverClient.Log;

namespace MarsRoverClient.Pages
{
    public class LogViewModel : INotifyPropertyChanged
    {
        #region Properties

        public LogListViewModel LogListVM { get; set; }
        public LogLevel LogLevel 
        {
            get { return ApplicationLogger.Instance.LogLevel; }
            protected set
            { 
                ApplicationLogger.Instance.LogLevel = (LogLevel)value;
                OnPropertyChanged("LogLevel");
                ApplicationLogger.Instance.Log(String.Format("Log Level has been changed to {0}", ApplicationLogger.Instance.LogLevel), LogLevel.Essential);
            }
        }

        #endregion
        
        #region Commands

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public LogViewModel()
        {
            
        }

        #region Command Methods
        

        #endregion

        #region Event Handlers

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
