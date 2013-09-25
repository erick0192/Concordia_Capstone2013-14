using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MRClient_ModernUIProtoss.Log;

namespace MRClient_ModernUIProtoss.Content
{
    class CameraViewModel : DependencyObject, INotifyPropertyChanged
    {
        #region Properties
        
        public string CameraName { get; set; }

        private bool mIsActive = true;
        public bool IsActive
        {
            get { return mIsActive; }
            set 
            { 
                mIsActive = value; 
                OnPropertyChanged("IsActive");
                ApplicationLogger.Instance.Log(String.Format("Camera \"{0}\" has been {1}", CameraName, IsActive ? "actived": "de-activated"), LogLevel.Essential);
            }
        }

        #endregion

        #region Commands

        private ICommand mToggleCamera;
        public ICommand ToggleCamera
        {
            get
            {
                if (mToggleCamera == null)
                {
                    mToggleCamera = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ToggleCam(),
                        p => this.CanToggleCamera());
                }
                return mToggleCamera;
            }
        }

        #endregion        
       
        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public CameraViewModel()
        {
           
        }

        public CameraViewModel(string iCameraName)
        {
            CameraName = iCameraName;
        }

        #endregion

        #region Command Methods

        private bool CanToggleCamera() { return true; }

        private void ToggleCam()
        {            
            IsActive = !IsActive;
        }

        #endregion

        #region Event Handlers

        protected void OnPropertyChanged(string iPropertyName)
        {            
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(iPropertyName));
            }
        }

        #endregion
    }
}
