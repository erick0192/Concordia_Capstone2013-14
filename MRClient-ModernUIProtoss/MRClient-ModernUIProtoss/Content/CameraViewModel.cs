using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MarsRoverClient.Log;

namespace MarsRoverClient.Content
{
    class CameraViewModel : INotifyPropertyChanged
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

        private bool mIsExpanded = false;
        public bool IsExpanded
        {
            get { return mIsExpanded; }
            protected set
            {
                mIsExpanded = value;
                OnPropertyChanged("IsExpanded");
                ApplicationLogger.Instance.Log(String.Format("Camera \"{0}\" has been {1}", CameraName, IsExpanded ? "expanded" : "collapsed"), LogLevel.Debug);
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

        private ICommand mExpandViewCommand;
        public ICommand ExpandViewCommand
        {
            get
            {
                if (mExpandViewCommand == null)
                {
                    mExpandViewCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ExpandView(),
                        p => this.CanExpandView());
                }
                return mExpandViewCommand;
            }
        }

        private ICommand mCollapseViewCommand;
        public ICommand CollapseViewCommand
        {
            get
            {
                if (mCollapseViewCommand == null)
                {
                    mCollapseViewCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.CollapseView(),
                        p => this.CanCollapseView());
                }
                return mCollapseViewCommand;
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
        private bool CanExpandView() { return !IsExpanded; }
        private bool CanCollapseView() { return IsExpanded; }

        private void ToggleCam()
        {            
            IsActive = !IsActive;
        }

        private void ExpandView()
        {
            IsExpanded = true;
        }

        private void CollapseView()
        {
            IsExpanded = false;
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
