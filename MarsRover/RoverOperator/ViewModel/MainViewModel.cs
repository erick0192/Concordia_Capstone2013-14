using System.Windows.Input;
using RoverOperator.Content;
using System.IO;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using System.ComponentModel;

namespace RoverOperator.Pages
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Delegates

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Attributes

  

        #endregion

        #region Properties

        public Xceed.Wpf.AvalonDock.DockingManager DockingManager { get; set; }
        public CameraViewModel VMCamera1 { get; set; }
        public CameraViewModel VMCamera2 { get; set; }
        public CameraViewModel VMCamera3 { get; set; }

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
            var camNum = iParam as string;

            if ("1" == camNum)
            {                
                cvm = VMCamera1;
            }
            else if ("2" == camNum)
            {
                cvm = VMCamera2;
            }
            else if ("3" == camNum)
            {
                cvm = VMCamera3;
            }

            if (cvm != null) return cvm.ToggleCamera.CanExecute(null);
            return false;
        }

        protected void ToggleCamera(object iParam)
        {
            CameraViewModel cvm = null;
            var camNum = iParam as string;

            if ("1" == camNum)
            {
                cvm = VMCamera1;
            }
            else if ("2" == camNum)
            {
                cvm = VMCamera2;
            }
            else if ("3" == camNum)
            {
                cvm = VMCamera3;
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

        #region Methods

        #endregion

       
    }
}
