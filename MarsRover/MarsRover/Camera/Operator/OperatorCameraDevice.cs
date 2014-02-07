using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public class OperatorCameraDevice : AbstractCameraDevice
    {

        protected int DoubleBufferFree = 0;
        volatile protected Bitmap DoubleBufferFrameA;
        volatile protected Bitmap DoubleBufferFrameB;


        public OperatorCameraDevice()
        {                                  
            State = CameraState.CAMERA_STOPPED;
        }

        public Bitmap GetLatestFrame()
        {
            if (DoubleBufferFree == 0)
            {
                return DoubleBufferFrameB;
            }
            else
            {
                return DoubleBufferFrameA;
            }
        }

        public void SetLatestFrame(Bitmap aBitmap)
        {
            if (DoubleBufferFree == 0)
            {
                DoubleBufferFrameA = aBitmap;

                DoubleBufferFree = 1;
            }
            else
            {
                DoubleBufferFrameB = aBitmap;

                DoubleBufferFree = 0;
            }
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
