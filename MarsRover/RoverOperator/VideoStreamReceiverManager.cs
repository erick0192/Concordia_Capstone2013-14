using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;

namespace MarsRover.Streams
{
    public class VideoStreamReceiverManager
    {
        #region Private Members
        
        //private FilterInfoCollection mVideoDevices;
        private IVideoSource camera1;
        private IVideoSource camera2;
        private IVideoSource camera3;

        #endregion

        #region Properties

        private static VideoStreamReceiverManager mInstance;
        public static VideoStreamReceiverManager Instance
        {
            get
            {
                if (null == mInstance)
                    mInstance = new VideoStreamReceiverManager();

                return mInstance;
            }
        }

        public IVideoSource Camera1
        {
            get
            {
                if (null == camera1)
                {
                    //mFrontCameraStream = new VideoCaptureDevice(mVideoDevices[0].MonikerString);
                    camera1 = new VideoStreamReceiver(
                        RoverOperator.NetworkSettings.Instance.RoverIPAddress,
                        RoverOperator.NetworkSettings.Instance.CameraPort1);
                }

                return camera1;
            }
        }

        public IVideoSource Camera2
        {
            get
            {
                if (null == camera2)
                {
                    camera2 = new VideoStreamReceiver(
                        RoverOperator.NetworkSettings.Instance.RoverIPAddress,
                        RoverOperator.NetworkSettings.Instance.CameraPort2);
                }

                return camera2;
            }
        }

        public IVideoSource Camera3
        {
            get
            {
                if (null == camera3)
                {
                    camera3 = new VideoStreamReceiver(
                        RoverOperator.NetworkSettings.Instance.RoverIPAddress, 
                        RoverOperator.NetworkSettings.Instance.CameraPort3);
                }

                return camera3;
            }
        }

        #endregion

        private VideoStreamReceiverManager()
        {
            //mVideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }             

        //Make sure to call this method when the application is closing.
        //Otherwise, streams will be left open and the appliction will not be shut down, even though the GUI has
        public void StopAllStreams()
        {
            if (null != camera1 && camera1.IsRunning)
                camera1.SignalToStop();

            if (null != camera2 && camera2.IsRunning)
                camera2.SignalToStop();

            if (null != camera3 && camera3.IsRunning)
                camera3.SignalToStop();

        }
    }
}
