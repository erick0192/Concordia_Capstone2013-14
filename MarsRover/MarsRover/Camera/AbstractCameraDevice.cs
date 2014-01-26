using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public enum CameraState
    {
        CAMERA_STARTED,
        CAMERA_STOPPED
    };
   
    public class AbstractCameraDevice
    {
        protected CameraState State;

        protected RoverCameraDeviceStatistics Statistics;

        protected Bitmap LatestFrame;

        protected int FrameNumber;

        public float GetFPS()
        {
            return Statistics.GetCalculatedFPS();
        }

        public CameraState GetState()
        {
            return State;
        }

        public virtual void Start()
        {

        }

        public virtual void Stop()
        {

        }
    }
}
