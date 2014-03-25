using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
            protected set
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

        private UDPOperatorCameraDevice mVideoSource;
        public UDPOperatorCameraDevice VideoSource
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
                        p => this.ToggleCam(p),
                        p => this.CanToggleCamera(p));
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
            toggleTimer = new System.Timers.Timer(500);
            toggleTimer.AutoReset = false;
            toggleTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.EnableToggle);

            logger = NLog.LogManager.GetLogger("Console");
            
        }

        #endregion

        #region Command Methods

        private bool CanToggleCamera(object param) 
        {
            return canToggle;
        }

        private void ToggleCam(object param)
        {
            //For a reason this method gets called 3 time then I press CTRL+[CamID]
            if (mVideoSource.GetState() == CameraState.CAMERA_STARTED)
            {               
                mVideoSource.aNewBitmapReceivedEvent -= new UDPOperatorCameraDevice.NewBitmapReceivedCBType(HandleNewVideoFrame);
                //Call this method to stop the camera remotly
                mVideoSource.Stop();
                IsActive = false;              
            }
            else
            {
                mVideoSource.aNewBitmapReceivedEvent += new UDPOperatorCameraDevice.NewBitmapReceivedCBType(HandleNewVideoFrame);
                //Call this method to start the camera remotly
                mVideoSource.Start();
                IsActive = true;               
            }
         
            canToggle = false;
            toggleTimer.Start();           
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
                //This is where we are having a lot of troubles, we need to make sure that while we are updating a 
                //image on the UI we do not overwrite the image with a new one.
                //We will need to find a solution together?
                if (IsUpdating == false)
                {
                    IsUpdating = true;
                    
                    //Thread.Sleep(100);
                    
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
