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
        private bool isInitialized = false;
        private List<RoverCameraDevice> Cameras = new List<RoverCameraDevice>();

        private RoverCameraFactory()
        {
        
        }

        public void Initialize(string Port, int BasePort)
        {
            Cameras.Clear();

            for (int i = 0; i < RoverCameraDetector.GetInstance().GetCameraDevices().Count; i++)
            {
                FilterInfo fx = (FilterInfo)RoverCameraDetector.GetInstance().GetCameraDevices()[i];
                RoverCameraDevice cx = new UDPRoverCameraDevice(Port, BasePort + i, fx.Name, fx.MonikerString, 0, 50L, 3);
                Cameras.Add(cx);
            }

            isInitialized = true;

        }

        public static RoverCameraFactory GetInstance()
        {
            if (SingletonCameraFactory == null)
            {
                SingletonCameraFactory = new RoverCameraFactory();                
            }

            return SingletonCameraFactory;
        }
        
        public bool IsInitialized()
        {
            return isInitialized;
        }
        
        public List<RoverCameraDevice> GetCameras()
        {
            return Cameras;
        }

    }
}
