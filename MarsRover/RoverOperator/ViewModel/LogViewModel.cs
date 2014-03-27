using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using NLog;
using RoverOperator.Log;
using System.Windows.Controls;
using System;

namespace RoverOperator.Pages
{
    public class LogViewModel : LogEventObserver
    {
        #region Properties

        public System.Windows.Controls.ListView LogMessagesControl;
        private Dictionary<string, bool> filteringList;
        private int lastClear;
        private Logger logger;

        #endregion

        #region Commands

        private ICommand mToggleFilterCommand;
        public ICommand ToggleFilterCommand
        {
            get
            {
                if (mToggleFilterCommand == null)
                {
                    mToggleFilterCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        c => this.ToggleFilter(c));
                }
                return mToggleFilterCommand;
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
                        c => this.ClearLog(c));
                }
                return mClearLogCommand;
            }
        }

        #endregion

        #region Delegates and Events

        #endregion

        public LogViewModel(Log view)
        {
            filteringList = new Dictionary<string, bool>(6);
            filteringList["Trace"] = true;
            filteringList["Debug"] = true;
            filteringList["Info"] = true;
            filteringList["Warn"] = true;
            filteringList["Error"] = true;
            filteringList["Fatal"] = true;

            LogEventSubject.Attach(this);
            logger = LogManager.GetCurrentClassLogger();
            view.Loaded += view_Loaded;
        }

        #region Command Methods


        #endregion

        #region Methods

        public void RefreshLogList()
        {
            ApplyFiltering();
        }

        private void ToggleFilter(object p)
        {
            CheckBox chk = (CheckBox)p;
            string logLevel = chk.Content.ToString();
            filteringList[logLevel] = (bool)chk.IsChecked;
            logger.Log(LogLevel.FromString(logLevel), logLevel + " filtering " + (((bool)chk.IsChecked) ? "on" : "off"));
            ApplyFiltering();
        }

        private void ApplyFiltering()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                LogMessagesControl.ItemsSource = LogEventSubject.Events.Skip(lastClear).Where(ev => filteringList[ev.Level]);
            });
        }

        private void ClearLog(object p)
        {
            lastClear = LogEventSubject.Events.Count;
            LogMessagesControl.ItemsSource = LogEventSubject.Events.Skip(lastClear);
        }

        private void view_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LogMessagesControl.ItemsSource = LogEventSubject.Events;
        }

        #endregion
    }

}
