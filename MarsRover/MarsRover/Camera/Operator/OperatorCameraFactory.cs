using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MarsRover
{
    public class OperatorCameraFactory
    {
        private static OperatorCameraFactory SingletonCameraFactory;

        private List<OperatorCameraDevice> Cameras = new List<OperatorCameraDevice>();

        private OperatorCameraFactory()
        {

            //RemoteCameraDevice c0 = new UDPListenerCameraDevice("127.0.0.1", 3000);
            //Cameras.Add(c0);

            //RemoteCameraDevice c1 = new UDPListenerCameraDevice("127.0.0.1", 3001);
            //Cameras.Add(c1);            

        }

        public static OperatorCameraFactory GetInstance()
        {
            if (SingletonCameraFactory == null)
            {
                SingletonCameraFactory = new OperatorCameraFactory();                
            }

            return SingletonCameraFactory;
        }

        public List<OperatorCameraDevice> GetCameras()
        {
            return Cameras;
        }

    }
}
