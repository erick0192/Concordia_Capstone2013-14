using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MRClient_ModernUIProtoss.Content;

namespace MRClient_ModernUIProtoss.Pages
{
    class MainViewModel
    {
        public CameraViewModel UpperLeftCameraVM;
        public CameraViewModel UpperRightCameraVM;
        public CameraViewModel LowerLeftCameraVM;
        public CameraViewModel LowerRightCameraVM;

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

        public MainViewModel()
        {

        }
    }
}
