using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Video.DirectShow;

namespace MarsRover
{
    public class RoverCameraFactory
    {
        private static RoverCameraFactory SingletonCameraFactory;

        private List<RoverCameraDevice> Cameras = new List<RoverCameraDevice>();

        private RoverCameraFactory()
        {
            for (int i = 0; i < RoverCameraDetector.GetInstance().GetCameraDevices().Count; i++)
            {
                FilterInfo fx = (FilterInfo)RoverCameraDetector.GetInstance().GetCameraDevices()[i];
                RoverCameraDevice cx = new UDPRoverCameraDevice(Properties.NetworkSetting.Default.OperatorIP, Properties.NetworkSetting.Default.CameraBasePort + i, fx.Name, fx.MonikerString, 0, 50L);
                Cameras.Add(cx);
            }

            //FilterInfo f1 = (FilterInfo)RoverCameraDetector.GetInstance().GetCameraDevices()[1];
            //RoverCameraDevice c1 = new UDPSenderCameraDevice("127.0.0.1", 3001, f1.Name, f1.MonikerString, 0, 50L);
            //Cameras.Add(c1);

        }

        public static RoverCameraFactory GetInstance()
        {
            if (SingletonCameraFactory == null)
            {
                SingletonCameraFactory = new RoverCameraFactory();                
            }

            return SingletonCameraFactory;
        }

        public List<RoverCameraDevice> GetCameras()
        {
            return Cameras;
        }

    }
}
