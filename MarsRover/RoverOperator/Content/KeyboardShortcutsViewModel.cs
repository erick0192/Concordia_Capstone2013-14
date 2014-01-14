using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverOperator.Content
{
    public class KeyboardShortCut
    {
        public String Modifier { get; private set; }
        public String Key { get; private set; }
        public String Action { get; private set; }

        public KeyboardShortCut(String modifier,
            String key, String action)
        {
            Modifier = modifier;
            Key = key;
            Action = action;
        }
    }

    public class KeyboardShortcutsViewModel : INotifyPropertyChanged
    {
        #region Private fields

        #endregion

        #region Properties

        private List<KeyboardShortCut> appShortcuts;
        public List<KeyboardShortCut> AppShortcuts
        {
            get
            {
                return appShortcuts;
            }
            set
            {
                appShortcuts = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AppShortcuts"));
                }
            }
        }

        private List<KeyboardShortCut> mainPageShortcuts;
        public List<KeyboardShortCut> MainPageShortcuts
        {
            get
            {
                return mainPageShortcuts;
            }
            set
            {
                mainPageShortcuts = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MainPageShortcuts"));
                }
            }
        }
        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        public KeyboardShortcutsViewModel()
        {
            var appSC = new List<KeyboardShortCut>();
            appSC.Add(new KeyboardShortCut("Ctrl", "H", "Navigate to the Home/Main page."));
            appSC.Add(new KeyboardShortCut("Ctrl", "L", "Navigate to the Log page."));
            appSC.Add(new KeyboardShortCut("Ctrl", "T", "Navigate to the Settings page."));
            appSC.Add(new KeyboardShortCut("", "F12", "Navigate to the Help page."));

            var mainSC = new List<KeyboardShortCut>();
            mainSC.Add(new KeyboardShortCut("Ctrl", "1", "Show/Hide the front camera view. This will also start/stop the video source behind it."));
            mainSC.Add(new KeyboardShortCut("Ctrl", "2", "Show/Hide the back camera view. This will also start/stop the video source behind it."));
            mainSC.Add(new KeyboardShortCut("Ctrl", "3", "Show/Hide the left camera view. This will also start/stop the video source behind it."));
            mainSC.Add(new KeyboardShortCut("Ctrl", "4", "Show/Hide the right camera view. This will also start/stop the video source behind it."));

            MainPageShortcuts = mainSC;
            AppShortcuts = appSC;
        }

        #endregion
    }
}
