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
        private UDPListenerCameraDevice camera1;
        private UDPListenerCameraDevice camera2;
        private UDPListenerCameraDevice camera3;

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

        public UDPListenerCameraDevice Camera1
        {
            get
            {
                if (null == camera1)
                {
                  camera1 = new UDPListenerCameraDevice(RoverOperator.NetworkSettings.Instance.RoverIPAddress, RoverOperator.NetworkSettings.Instance.CameraPort1);                  
                }

                return camera1;
            }
        }

        public UDPListenerCameraDevice Camera2
        {
            get
            {
                if (null == camera2)
                {                  
                    camera2 = new UDPListenerCameraDevice(RoverOperator.NetworkSettings.Instance.RoverIPAddress, RoverOperator.NetworkSettings.Instance.CameraPort2);                

                }

                return camera2;
            }
        }

        public UDPListenerCameraDevice Camera3
        {
            get
            {
                if (null == camera3)
                {
                    camera2 = new UDPListenerCameraDevice(RoverOperator.NetworkSettings.Instance.RoverIPAddress, RoverOperator.NetworkSettings.Instance.CameraPort3);                
                }

                return camera3;
            }
        }

        #endregion

        private VideoStreamReceiverManager()
        {
            
        }             
        
    }
}
