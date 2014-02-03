// Snapshot Maker sample application
// AForge.NET Framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2009-2011
// contacts@aforgenet.com
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using AForge.Video;
using AForge.Video.DirectShow;


namespace MarsRover
{

    public class RoverCameraDetector
    {
        private static RoverCameraDetector SingletonCameraManager;

        private FilterInfoCollection VideoDevices;
        private ArrayList AvailableCameraDevices;

        public static RoverCameraDetector GetInstance()
        {
            if (SingletonCameraManager == null)
            {
                SingletonCameraManager = new RoverCameraDetector();
            }

            return SingletonCameraManager;
        }


        private RoverCameraDetector()
        {
            AvailableCameraDevices = new ArrayList();

            ScanVideoDevices();
        }

      
        private void ScanVideoDevices()
        {
            AvailableCameraDevices.Clear();
           
            VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
            if (VideoDevices.Count == 0)
            {
                Console.WriteLine("No camera was found");
            }
            else
            {
                Console.WriteLine(VideoDevices.Count + " Cameras were found");
                
                foreach ( FilterInfo device in VideoDevices )
                {                    
                    AvailableCameraDevices.Add(device);
                }
            }
            
        }

        public ArrayList GetCameraDevices()
        {
            return AvailableCameraDevices;
        }


    }
}
