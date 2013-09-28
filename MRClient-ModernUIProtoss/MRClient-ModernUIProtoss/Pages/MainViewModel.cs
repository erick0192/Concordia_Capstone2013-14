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

        #endregion

        #region Commands

        private ICommand mToggleCameraCommand;        
        public ICommand ToggleCameraCommand
        {
            get
            {
                if (mToggleCameraCommand == null)
                {
                    mToggleCameraCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ToggleCamera(p),
                        p => this.CanToggleCamera(p));
                }
                return mToggleCameraCommand;
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

        protected bool CanToggleCamera(object iParam)
        {
            CameraViewModel cvm = null;

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

            return cvm.ToggleCamera.CanExecute(null) ;
        }

        protected void ToggleCamera(object iParam)
        {
            CameraViewModel cvm = null;

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

            cvm.ToggleCamera.Execute(null);
        }

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
            {
                cvm.CollapseViewCommand.Execute(null);
            }
            else
            {
                //If another view is expanded, collapse it before expanding the desired one.
                if (cvm != UpperLeftCameraVM && UpperLeftCameraVM.IsExpanded)
                    UpperLeftCameraVM.CollapseViewCommand.Execute(null);
                else if (cvm != UpperRightCameraVM && UpperRightCameraVM.IsExpanded)
                    UpperRightCameraVM.CollapseViewCommand.Execute(null);
                else if (cvm != LowerLeftCameraVM && LowerLeftCameraVM.IsExpanded)
                    LowerLeftCameraVM.CollapseViewCommand.Execute(null);
                else if (cvm != LowerRightCameraVM && LowerRightCameraVM.IsExpanded)
                    LowerRightCameraVM.CollapseViewCommand.Execute(null);

                cvm.ExpandViewCommand.Execute(null);
            }
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
