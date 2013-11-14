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
        private Dictionary<string, bool> filteringList;

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
            filteringList[chk.Content.ToString()] = (bool)chk.IsChecked;
            ApplyFiltering();
        }

        private void ApplyFiltering()
        {
            LogMessagesControl.ItemsSource = LogEventSubject.Events.Where(ev => filteringList[ev.Level]);
        }

        private void view_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LogMessagesControl.ItemsSource = LogEventSubject.Events;
        }

        #endregion
    }

}
