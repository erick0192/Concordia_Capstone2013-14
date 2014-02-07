using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Timers;
using AForge.Video;
using AForge.Video.DirectShow;


namespace MarsRover
{
    
    public class RoverCameraDevice : AbstractCameraDevice
    {
      
        public delegate void NewBitmapAcquiredCBType(Bitmap aNewBitmap);
        public event NewBitmapAcquiredCBType NewBitmapAcquiredCBHandler;
        
        private string CameraName;
        private int CameraID;
        private int FrameRateDivider;

        private VideoCaptureDevice videoDevice;
        private VideoCapabilities videoDeviceCapabilities;
       

        //This function gets called when a new images is received by this cameraDevice
        private void NewFrameAcquired(object sender, NewFrameEventArgs eventArgs)
        {
            FrameNumber++;
            //Console.WriteLine("[" + CameraID + "]: " + CameraName + " frame " + FrameNumber + " received");

            if (FrameNumber % FrameRateDivider == 0)
            {                
                SetLatestFrame((Bitmap)eventArgs.Frame.Clone());

                if (NewBitmapAcquiredCBHandler != null)
                {
                    NewBitmapAcquiredCBHandler(LatestFrame);
                }
            }
            
            
        }

        public RoverCameraDevice(string aCameraName, string MonikerString, int aCameraID, int aFrameRateDivider)
        {            
            CameraName = aCameraName;
            CameraID = aCameraID;
            videoDevice = new VideoCaptureDevice(MonikerString);
            FrameRateDivider = aFrameRateDivider;

           // Console.WriteLine("[" + CameraID + "]: " + CameraName + " created ");

            videoDevice.NewFrame += new NewFrameEventHandler(NewFrameAcquired);

            Statistics = new RoverCameraDeviceStatistics(this, 1000);

            State = CameraState.CAMERA_STOPPED;
        }

        public void SetLatestFrame(Bitmap aBitmap)
        {
            LatestFrame = aBitmap;
        }

        public string GetName()
        {
            return CameraName;
        }

        public int GetID()
        {
            return CameraID;
        }

        public void RegisterBitmapAcquiredCB(NewBitmapAcquiredCBType aNewBitmapAcquiredCBHandler)
        {
            this.NewBitmapAcquiredCBHandler += new NewBitmapAcquiredCBType(aNewBitmapAcquiredCBHandler);
        }

        public void Start(int DesiredFrameRate, Size FrameSize)
        {
            FrameNumber = 0;
            videoDevice.DesiredFrameRate = DesiredFrameRate;
            videoDevice.DesiredFrameSize = FrameSize;            
            videoDevice.Start();

            State = CameraState.CAMERA_STARTED;
        }

        public void Start(VideoCapabilities Capability)
        {
            FrameNumber = 0;            
            videoDevice.VideoResolution = Capability;
            videoDevice.Start();

            State = CameraState.CAMERA_STARTED;
        }

        public override void Stop()
        {
            if (!(videoDevice == null))
            {
                if (videoDevice.IsRunning)
                {
                    videoDevice.SignalToStop();                   

                    ResetFrameNumber();

                    State = CameraState.CAMERA_STOPPED;
                }
            }

        }

        public int GetFrameNumber()
        {
            return FrameNumber;
        }

        public void ResetFrameNumber()
        {
            FrameNumber = 0;
        }
       

        public VideoCapabilities GetCapabilities(Size Resolution)
        {
            for (int i = 0; i < videoDevice.VideoCapabilities.Length; i++)
            {
                if (videoDevice.VideoCapabilities[i].FrameSize.Equals(Resolution) == true)
                {
                    return videoDevice.VideoCapabilities[i];
                }
            }

            return videoDevice.VideoCapabilities[0];

        }

        public VideoCapabilities GetCapabilities(int ConfigIndex)
        {
            return videoDevice.VideoCapabilities[ConfigIndex];
        }

        public int GetAverageFrameRate(int ConfigIndex)
        {
            return videoDevice.VideoCapabilities[ConfigIndex].AverageFrameRate;
        }

        public int GetMaximumFrameRate(int ConfigIndex)
        {
            return videoDevice.VideoCapabilities[ConfigIndex].MaximumFrameRate;
        }

        public Size GetSupportedFrameSize(int ConfigIndex)
        {
            return videoDevice.VideoCapabilities[ConfigIndex].FrameSize;
        }

        public int GetNumberOfConfiguration()
        {
            return videoDevice.VideoCapabilities.Length;
        }

        public void PrintConfigurationInfo()
        {
            Console.WriteLine(CameraName + "Support "+ GetNumberOfConfiguration() + " Different configurations");

            for (int i = 0; i < GetNumberOfConfiguration(); i++)
            {
                Console.WriteLine("Configuration: " + i);
                Console.WriteLine("Average FrameRate:" + GetAverageFrameRate(i));
                Console.WriteLine("Maximum FrameRate:" + GetMaximumFrameRate(i));
                Console.WriteLine("SupportedFrameSize:" + GetSupportedFrameSize(i).ToString());
            }
        }
        
    }
}
