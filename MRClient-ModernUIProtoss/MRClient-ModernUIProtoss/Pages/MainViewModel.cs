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

        private ICommand mToggleUpperRightCam;
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

        private ICommand mToggleLowerLeftCam;
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

        private ICommand mToggleLowerRightCam;        
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

        private ICommand mExpandCameraViewCommand;
        public ICommand ExpandCameraViewCommand
        {
            get
            {
                if (mExpandCameraViewCommand == null)
                {
                    mExpandCameraViewCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => ExpandCameraView(p),
                        p => CanExpandCameraView(p));
                }
                return mExpandCameraViewCommand;
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            //ApplicationLogger.Instance.LogEntryEvent += new ApplicationLogger.LogEntryHandler(AddLogEntry);
        }

        #endregion

        #region Command Methods

        protected bool CanExpandCameraView(object iParam)
        {
            CameraViewModel cvm = null;
            bool canExecute = false;

            if ("front" == (string)iParam)
            {
                cvm = UpperLeftCameraVM;
            }
            else if ("back" == (string)iParam)
            {
                cvm = UpperRightCameraVM;
            }
            else if ("left" == (string)iParam)
            {
                cvm = LowerLeftCameraVM;
            }
            else if ("right" == (string)iParam)
            {
                cvm = LowerRightCameraVM;
            }

            canExecute = cvm.IsExpanded? cvm.CollapseViewCommand.CanExecute(null): cvm.ExpandViewCommand.CanExecute(null);

            return canExecute;
        }

        protected void ExpandCameraView(object iParam)
        {
            CameraViewModel cvm = null ;

            if ("front" == (string)iParam)
            {
                cvm = UpperLeftCameraVM;
            }
            else if ("back" == (string)iParam)
            {
                cvm = UpperRightCameraVM;
            }
            else if ("left" == (string)iParam)
            {
                cvm = LowerLeftCameraVM;
            }
            else if ("right" == (string)iParam)
            {
                cvm = LowerRightCameraVM;
            }

            if (cvm.IsExpanded)
                cvm.CollapseViewCommand.Execute(null);
            else
                cvm.ExpandViewCommand.Execute(null);
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
