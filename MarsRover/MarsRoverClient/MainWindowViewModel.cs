using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using FirstFloor.ModernUI.Windows.Controls;
using MarsRoverClient.Content;
using NLog;

namespace MarsRoverClient
{
    public class MainWindowViewModel
    {
        #region Properties        

        #endregion

        #region Commands

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

        private ICommand mGoToLogCommand;
        public ICommand GoToLogCommand
        {
            get
            {
                if (mGoToLogCommand == null)
                {
                    mGoToLogCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.GoToLog(p),
                        p => this.CanExecute(p));
                }
                return mGoToLogCommand;
            }
        }

        #endregion

        #region Constructor

        public MainWindowViewModel() { }

        #endregion

        #region Command Methods

        private bool CanExecute(object iParam)
        {
            return true;
        }

        private void GoToHome(object iParam)
        {
            ModernWindow mainWindow = (ModernWindow)iParam;
            mainWindow.ContentSource = new Uri(@"/Pages/Main.xaml", UriKind.Relative);                        
        }

        private void GoToSettings(object iParam)
        {
            ModernWindow mainWindow = (ModernWindow)iParam;
            mainWindow.ContentSource = new Uri(@"/Pages/Settings.xaml", UriKind.Relative);
        }

        private void GoToHelp(object iParam)
        {
            ModernWindow mainWindow = (ModernWindow)iParam;
            mainWindow.ContentSource = new Uri(@"/Pages/Help.xaml", UriKind.Relative);
        }

        private void GoToLog(object iParam)
        {
            ModernWindow mainWindow = (ModernWindow)iParam;
            mainWindow.ContentSource = new Uri(@"/Pages/Log.xaml", UriKind.Relative);
        }

        #endregion 
    }
}
