using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MarsRoverClient.Content;
using NLog;
using MarsRoverClient.Log;
using System.Windows.Controls;

namespace MarsRoverClient.Pages
{
    public class LogViewModel : LogEventObserver
    {
        #region Properties

        public System.Windows.Controls.ListView LogMessagesControl;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Commands

        #endregion

        #region Delegates and Events


        #endregion

        public LogViewModel()
        {
            LogEventSubject.Attach(this);
            logger.Debug("LOL");
        }

        #region Command Methods


        #endregion

        #region Methods

        public void UpdateLogList(string longdate, string level, string callsite, string message)
        {
            if (LogMessagesControl != null)
            {
                LogMessagesControl.Items.Add(new { Date = longdate, Level = level, Callsite = callsite, Message = message });
            }
        }

        #endregion
    }

}
