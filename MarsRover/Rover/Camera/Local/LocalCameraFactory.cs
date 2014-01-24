using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Video.DirectShow;

namespace Rover
{
    public class LocalCameraFactory
    {
        private static LocalCameraFactory SingletonCameraFactory;

        private List<LocalCameraDevice> Cameras = new List<LocalCameraDevice>();

        private LocalCameraFactory()
        {

            FilterInfo f0 = (FilterInfo)LocalCameraDetector.GetInstance().GetCameraDevices()[0];
            LocalCameraDevice c0 = new UDPSenderCameraDevice("127.0.0.1", 3000, f0.Name, f0.MonikerString, 0);
            Cameras.Add(c0);

            FilterInfo f1 = (FilterInfo)LocalCameraDetector.GetInstance().GetCameraDevices()[1];
            LocalCameraDevice c1 = new UDPSenderCameraDevice("127.0.0.1", 3001, f1.Name, f1.MonikerString, 0);
            Cameras.Add(c1);

        }

        public static LocalCameraFactory GetInstance()
        {
            if (SingletonCameraFactory == null)
            {
                SingletonCameraFactory = new LocalCameraFactory();                
            }

            return SingletonCameraFactory;
        }

        public List<LocalCameraDevice> GetCameras()
        {
            return Cameras;
        }

    }
}
