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
using MarsRoverClient.Log;

namespace MarsRoverClient.Content
{
    class CameraViewModel : INotifyPropertyChanged
    {
        //TaskFactory mUIFactory;

        #region Properties
        
        public string CameraName { get; set; }

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
                OnPropertyChanged("Image");
            }
        }

        private IVideoSource mVideoSource;
        public IVideoSource VideoSource
        {
            get
            {
                return mVideoSource;
            }
            set
            {
                if (null != mVideoSource)
                {
                    mVideoSource.NewFrame -= new NewFrameEventHandler(HandleNewVideoFrame);
                    mVideoSource.PlayingFinished -= new PlayingFinishedEventHandler(HandleFinishedPlaying);
                }

                mVideoSource = value;
                mVideoSource.NewFrame += new NewFrameEventHandler(HandleNewVideoFrame);
                mVideoSource.PlayingFinished += new PlayingFinishedEventHandler(HandleFinishedPlaying);
                OnPropertyChanged("VideoSource");                
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

        public CameraViewModel() : this("Camera")
        {
           
        }

        public CameraViewModel(string iCameraName)
        {
            CameraName = iCameraName;
            
            //mUIFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region Command Methods

        private bool CanToggleCamera() { return true; }

        private void ToggleCam()
        {
            //if (mIsActive)
            if(mVideoSource.IsRunning)
            {
                mVideoSource.SignalToStop();
                //Image = null;
            }
            else
            {             
                mVideoSource.Start();
            }
           
            //IsActive = !mIsActive;
            
        }

        #endregion

        #region Event Handlers

        private void HandleFinishedPlaying(object sender, ReasonToFinishPlaying reason)
        {
            Image = null;
        }

        protected void OnPropertyChanged(string iPropertyName)
        {            
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(iPropertyName));
            }
        }

        private void HandleNewVideoFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();

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
                    Image = bi;
                }));
                //Other method, however, if the application is close, exceptions are thrown due to aborting threads
                //mUIFactory.StartNew(() => Image = bi).Wait();            
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

    }
}
