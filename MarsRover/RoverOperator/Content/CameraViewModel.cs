using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AForge.Video;
using MarsRover;

namespace RoverOperator.Content
{
    public class CameraViewModel : INotifyPropertyChanged
    {
      
        #region Private attributes

        System.Timers.Timer toggleTimer;
        private volatile bool canToggle = true;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private volatile bool IsUpdating = false;
        
        #endregion

        #region Properties

        public string CameraName { get; set; }

        public bool mIsActive = false;
        public bool IsActive
        {
            get
            {
                return mIsActive;
            }
            set
            {
                mIsActive = value;                
                logger.Debug("Camera {0} is now {1}.", CameraName, IsActive ? "active": "inactive");                
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsActive"));
                }
            }
        }

        private BitmapImage mImage;
        public BitmapImage Image 
        {
            get
            {
                return mImage;
            }
            set
            {
                mImage = value;
                if (PropertyChanged != null)
                {
                    OnPropertyChanged("Image");
                }
            }
        }

        private UDPListenerCameraDevice mVideoSource;
        public UDPListenerCameraDevice VideoSource
        {
            get
            {
                return mVideoSource;
            }
            set
            {
                if (null != mVideoSource)
                {                    
                                    
                }

                mVideoSource = value;
               
                if (PropertyChanged != null)
                {
                    OnPropertyChanged("VideoSource");
                }
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
                    mToggleCamera = new RelayCommand(
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

        public CameraViewModel() : this("Camera")
        {
           
        }

        public CameraViewModel(string iCameraName)
        {
            CameraName = iCameraName;
             
            //logger = NLog.LogManager.GetLogger("Console");
            
        }

        #endregion

        #region Command Methods

        private bool CanToggleCamera() 
        {
            return canToggle;
        }

        private void ToggleCam()
        {
            if(IsActive == true)
            {
                //remove the event
                mVideoSource.aNewBitmapReceivedEvent -= new UDPListenerCameraDevice.NewBitmapReceivedCBType(HandleNewVideoFrame);
                IsActive = false;
            }
            else
            {
                //add the event
                mVideoSource.aNewBitmapReceivedEvent += new UDPListenerCameraDevice.NewBitmapReceivedCBType(HandleNewVideoFrame);
                IsActive = true;
            }
           
        }

        #endregion

        #region Event Handlers

        private void EnableToggle(object sender, System.Timers.ElapsedEventArgs e)
        {
            canToggle = true;
        }
      

        protected void OnPropertyChanged(string iPropertyName)
        {            
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(iPropertyName));
            }
        }

        private void HandleNewVideoFrame(Bitmap aBitmap)
        {
            try
            {

                if (IsUpdating == false)
                {
                    IsUpdating = true;
                    System.Drawing.Image img = (Bitmap)aBitmap.Clone();

                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.EndInit();
                    bi.Freeze();
                   

                    App.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        if (IsUpdating == false)
                        {
                            IsUpdating = true;
                            Image = bi;
                            IsUpdating = false;
                        }
                    }));

                    IsUpdating = false;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

    }
}
