using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MRClient_ModernUIProtoss.Content;
using MRClient_ModernUIProtoss.Log;

namespace MRClient_ModernUIProtoss.Pages
{
    class MainViewModel
    {
        #region Properties

        public CameraViewModel UpperLeftCameraVM { get; set; }
        public CameraViewModel UpperRightCameraVM { get; set; }
        public CameraViewModel LowerLeftCameraVM { get; set; }
        public CameraViewModel LowerRightCameraVM { get; set; }

        public bool IsViewExpanded { get; set; }

        #endregion

        #region Commands
        private ICommand mToggleUpperLeftCam;
        private ICommand mToggleUpperRightCam;
        private ICommand mToggleLowerLeftCam;
        private ICommand mToggleLowerRightCam;        

        public ICommand ToggleUpperLeftCamera
        {
            get
            {
                if (mToggleUpperLeftCam == null)
                {
                    mToggleUpperLeftCam = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.UpperLeftCameraVM.ToggleCamera.Execute(null),
                        p => this.UpperLeftCameraVM.ToggleCamera.CanExecute(null));
                }
                return mToggleUpperLeftCam;
            }
        }

        public ICommand ToggleUpperRightCamera
        {
            get
            {
                if (mToggleUpperRightCam == null)
                {
                    mToggleUpperRightCam = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.UpperRightCameraVM.ToggleCamera.Execute(null),
                        p => this.UpperRightCameraVM.ToggleCamera.CanExecute(null));
                }
                return mToggleUpperRightCam;
            }
        }

        public ICommand ToggleLowerLeftCamera
        {
            get
            {
                if (mToggleLowerLeftCam == null)
                {
                    mToggleLowerLeftCam = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.LowerLeftCameraVM.ToggleCamera.Execute(null),
                        p => this.LowerLeftCameraVM.ToggleCamera.CanExecute(null));
                }
                return mToggleLowerLeftCam;
            }
        }

        public ICommand ToggleLowerRightCamera
        {
            get
            {
                if (mToggleLowerRightCam == null)
                {
                    mToggleLowerRightCam = new FirstFloor.ModernUI.Presentation.RelayCommand(
                       p => this.LowerRightCameraVM.ToggleCamera.Execute(null),
                        p => this.LowerRightCameraVM.ToggleCamera.CanExecute(null));
                }
                return mToggleLowerRightCam;
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            //ApplicationLogger.Instance.LogEntryEvent += new ApplicationLogger.LogEntryHandler(AddLogEntry);
        }

        #endregion

        #region Event Handlers

        public void MainIsVisibleChanged(object iSender, System.Windows.DependencyPropertyChangedEventArgs iEventArgs)
        {
            if ((bool)iEventArgs.NewValue == true)
            {
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.ContextIdle,
                new Action(delegate()
                {
                    ((Main)iSender).Focus();
                }));
            }
        }

        public void AddLogEntry(LogEntry iLogEntry)
        {
            //FirstFloor.ModernUI.Windows.Controls.ModernDialog.ShowMessage(iLogEntry.ToString(), "", System.Windows.MessageBoxButton.OK);
            //System.Windows.MessageBox.Show(iLogEntry.ToString());
        }

        #endregion
    }
}
