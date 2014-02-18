using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using MarsRover;

namespace RoverOperator
{
    public class VideoStreamReceiverManager
    {
        #region Private Members
        
        //private FilterInfoCollection mVideoDevices;
        private UDPOperatorCameraDevice camera1;
        private UDPOperatorCameraDevice camera2;
        private UDPOperatorCameraDevice camera3;

        private static object syncRoot = new Object();

        #endregion

        #region Properties

        private static volatile VideoStreamReceiverManager instance;
        public static VideoStreamReceiverManager Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (null == instance)
                            instance = new VideoStreamReceiverManager();
                    }
                }

                return instance;
            }
        }

        public UDPOperatorCameraDevice Camera1
        {
            get
            {
                if (null == camera1)
                {
                  camera1 = new UDPOperatorCameraDevice(0, 
                      Properties.NetworkSettings.Default.RoverIPAddress,
                      Properties.NetworkSettings.Default.CameraPort1, 
                      Properties.NetworkSettings.Default.CommandsPort);                  
                }

                return camera1;
            }
        }

        public UDPOperatorCameraDevice Camera2
        {
            get
            {
                if (null == camera2)
                {
                    camera2 = new UDPOperatorCameraDevice(1,
                        Properties.NetworkSettings.Default.RoverIPAddress,
                        Properties.NetworkSettings.Default.CameraPort2, 
                        Properties.NetworkSettings.Default.CommandsPort);                

                }

                return camera2;
            }
        }

        public UDPOperatorCameraDevice Camera3
        {
            get
            {
                if (null == camera3)
                {
                    camera3 = new UDPOperatorCameraDevice(2,
                        Properties.NetworkSettings.Default.RoverIPAddress,
                        Properties.NetworkSettings.Default.CameraPort3, 
                        Properties.NetworkSettings.Default.CommandsPort);                
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
