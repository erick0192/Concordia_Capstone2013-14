using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public class OperatorCameraDevice : AbstractCameraDevice
    {

        public OperatorCameraDevice()
        {                                  
            State = CameraState.CAMERA_STOPPED;
        }

        public void SetLatestFrame(Bitmap aBitmap)
        {
            LatestFrame = aBitmap;
        }

        public Bitmap GetLastestFrame()
        {
            return LatestFrame;
        }

        public override void Start()
        {
            FrameNumber = 0;                     

            State = CameraState.CAMERA_STARTED;
        }

        public override void Stop()
        {
            if (State == CameraState.CAMERA_STARTED)
            {               
                ResetFrameNumber();

                State = CameraState.CAMERA_STOPPED;               
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
     
       
    }
}
