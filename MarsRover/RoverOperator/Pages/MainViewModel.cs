using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RoverOperator.Content;
using System.IO;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace RoverOperator.Pages
{
    public class MainViewModel
    {
        #region Properties

        public Xceed.Wpf.AvalonDock.DockingManager DockingManager { get; set; }
        public CameraViewModel VMCamera1 { get; set; }
        public CameraViewModel VMCamera2 { get; set; }
        public CameraViewModel VMCamera3 { get; set; }
        public CameraViewModel VMRightCamera { get; set; }

        #endregion

        #region Commands

        //Command to hide and turn off / show and turn on camera
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

        private ICommand mLoadLayoutCommand;
        public ICommand LoadLayoutCommand
        {
            get
            {
                if (mLoadLayoutCommand == null)
                {
                    mLoadLayoutCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => LoadLayout(p),
                        p => CanLoadLayout(p));
                }
                return mLoadLayoutCommand;
            }
        }

        private ICommand mSaveLayoutCommand;
        public ICommand SaveLayoutCommand
        {
            get
            {
                if (mSaveLayoutCommand == null)
                {
                    mSaveLayoutCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => SaveLayout(p),
                        p => CanSaveLayout(p));
                }
                return mSaveLayoutCommand;
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {

        }

        #endregion

        #region Command Methods

        protected bool CanToggleCamera(object iParam)
        {
            CameraViewModel cvm = null;

            if ("front" == (string)iParam)
            {                
                cvm = VMCamera1;
            }
            else if ("back" == (string)iParam)
            {
                cvm = VMCamera2;
            }
            else if ("left" == (string)iParam)
            {
                cvm = VMCamera3;
            }
            else if ("right" == (string)iParam)
            {
                cvm = VMRightCamera;
            }

            return cvm.ToggleCamera.CanExecute(null) ;
        }

        protected void ToggleCamera(object iParam)
        {
            CameraViewModel cvm = null;

            if ("front" == (string)iParam)
            {
                cvm = VMCamera1;
            }
            else if ("back" == (string)iParam)
            {
                cvm = VMCamera2;
            }
            else if ("left" == (string)iParam)
            {
                cvm = VMCamera3;
            }
            else if ("right" == (string)iParam)
            {
                cvm = VMRightCamera;
            }

            cvm.ToggleCamera.Execute(null);
        }

        protected bool CanLoadLayout(object iParam)
        {
            return File.Exists(@".\AvalonDock." + iParam + ".Layout.config");
        }

        protected void LoadLayout(object iParam)
        {
            var layoutSerializer = new XmlLayoutSerializer(DockingManager);
            layoutSerializer.Deserialize(@".\AvalonDock." + iParam + ".Layout.config");
        }

        protected bool CanSaveLayout(object iParam)
        {
            return true;
        }

        protected void SaveLayout(object iParam)
        {
            var layoutSerializer = new XmlLayoutSerializer(DockingManager);
            layoutSerializer.Serialize(@".\AvalonDock." + iParam + ".Layout.config");
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

        #endregion
    }
}
