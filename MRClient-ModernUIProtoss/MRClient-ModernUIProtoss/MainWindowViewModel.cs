using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;

namespace MRClient_ModernUIProtoss
{
    class MainWindowViewModel
    {
        private ICommand mGoToHomeCommand;
        public ICommand GoToHomeCommand
        {
            get
            {
                if (mGoToHomeCommand == null)
                {
                    mGoToHomeCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.GoToHome(p),
                        p => this.CanExecute(p));
                }
                return mGoToHomeCommand;
            }
        }

        private ICommand mGoToSettingsCommand;
        public ICommand GoToSettingsCommand
        {
            get
            {
                if (mGoToSettingsCommand == null)
                {
                    mGoToSettingsCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.GoToSettings(p),
                        p => this.CanExecute(p));
                }
                return mGoToSettingsCommand;
            }
        }

        private ICommand mGoToHelpCommand;
        public ICommand GoToHelpCommand
        {
            get
            {
                if (mGoToHelpCommand == null)
                {
                    mGoToHelpCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.GoToHelp(p),
                        p => this.CanExecute(p));
                }
                return mGoToHelpCommand;
            }
        }        

        public MainWindowViewModel()
        {

        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void GoToHome(object parameter)
        {
            FirstFloor.ModernUI.Windows.Controls.ModernWindow mainWindow = (FirstFloor.ModernUI.Windows.Controls.ModernWindow)parameter;
            mainWindow.ContentSource = new Uri(@"/Pages/Main.xaml", UriKind.Relative);                        
        }

        private void GoToSettings(object parameter)
        {
            FirstFloor.ModernUI.Windows.Controls.ModernWindow mainWindow = (FirstFloor.ModernUI.Windows.Controls.ModernWindow)parameter;
            mainWindow.ContentSource = new Uri(@"/Pages/Settings.xaml", UriKind.Relative);
        }

        private void GoToHelp(object parameter)
        {
            FirstFloor.ModernUI.Windows.Controls.ModernWindow mainWindow = (FirstFloor.ModernUI.Windows.Controls.ModernWindow)parameter;
            mainWindow.ContentSource = new Uri(@"/Pages/Help.xaml", UriKind.Relative);
        }
    }
}
