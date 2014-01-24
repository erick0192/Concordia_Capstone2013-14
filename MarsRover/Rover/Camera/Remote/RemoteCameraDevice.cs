using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Rover
{
    public class RemoteCameraDevice : AbstractCameraDevice
    {

        public RemoteCameraDevice()
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

        public void Start()
        {
            FrameNumber = 0;
          
            State = CameraState.CAMERA_STARTED;
        }

        public void Stop()
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
