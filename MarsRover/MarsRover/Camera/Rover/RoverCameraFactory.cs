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

            FilterInfo f0 = (FilterInfo)RoverCameraDetector.GetInstance().GetCameraDevices()[0];
            RoverCameraDevice c0 = new UDPSenderCameraDevice("127.0.0.1", 3000, f0.Name, f0.MonikerString, 0, 50L);
            Cameras.Add(c0);

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
