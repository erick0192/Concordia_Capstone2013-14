using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace MarsRover
{
    public class RoverCameraDeviceStatistics
    {
        private System.Timers.Timer FpsTimer;
        private RoverCameraDevice theCameraDevice;
        private float FPS;
        private int PreviousNumberOfFrames;
        private int TimerResolutionMiliSec;

        public RoverCameraDeviceStatistics(RoverCameraDevice aCameraDevice, int aTimerResolutionMiliSec)
        {
            theCameraDevice = aCameraDevice;
            TimerResolutionMiliSec = aTimerResolutionMiliSec;
            PreviousNumberOfFrames = 0;

            FpsTimer = new System.Timers.Timer();
            FpsTimer.Interval = TimerResolutionMiliSec;
            FpsTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            FpsTimer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            float DeltaFrames = theCameraDevice.GetFrameNumber() - PreviousNumberOfFrames;

            FPS = (float)(DeltaFrames / (float)(TimerResolutionMiliSec / 1000.0f));

            PreviousNumberOfFrames = theCameraDevice.GetFrameNumber();
        }

        public float GetCalculatedFPS()
        {
            return FPS;
        }

        public void Start()
        {
            PreviousNumberOfFrames = 0;
            FpsTimer.Start();
        }

        public void Stop()
        {
            PreviousNumberOfFrames = 0;
            FpsTimer.Stop();
        }
    }
}
