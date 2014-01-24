using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Rover
{
    public class RemoteCameraFactory
    {
        private static RemoteCameraFactory SingletonCameraFactory;

        private List<RemoteCameraDevice> Cameras = new List<RemoteCameraDevice>();

        private RemoteCameraFactory()
        {

            //RemoteCameraDevice c0 = new UDPListenerCameraDevice("127.0.0.1", 3000);
            //Cameras.Add(c0);

            //RemoteCameraDevice c1 = new UDPListenerCameraDevice("127.0.0.1", 3001);
            //Cameras.Add(c1);            

        }

        public static RemoteCameraFactory GetInstance()
        {
            if (SingletonCameraFactory == null)
            {
                SingletonCameraFactory = new RemoteCameraFactory();                
            }

            return SingletonCameraFactory;
        }

        public List<RemoteCameraDevice> GetCameras()
        {
            return Cameras;
        }

    }
}
