using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace MarsRover
{

    public class UDPOperatorCameraDevice : OperatorCameraDevice
    {
      
        public delegate void NewBitmapReceivedCBType(Bitmap aBitmap);
        public event NewBitmapReceivedCBType aNewBitmapReceivedEvent;

        private UDPListener aUDPListener;
        private UDPSender aUDPSender;

        private PacketReconstructor PacketReconstructors;
        private TypeConverter ImageConverter;

        private UDPListenerStatistics aUDPListenerStatistics;
        private int ID;

        
        public UDPOperatorCameraDevice(int ID, string IpAddress, int ListeningPort, int SendingPort)
        {
            PacketReconstructors = new PacketReconstructor(new Packet().GetBytes().Length, PacketReconstructedCBHandler);
            aUDPListener = new UDPListener(ListeningPort, ReceivedHandler);
            aUDPListenerStatistics = new UDPListenerStatistics(aUDPListener, 1000);

            aUDPSender = new UDPSender(IpAddress, SendingPort);
            
            ImageConverter = TypeDescriptor.GetConverter(typeof(Bitmap));
            this.ID = ID;
        }

        public void RegisterListener( NewBitmapReceivedCBType aNewBitmapReceivedCB)
        {
            aNewBitmapReceivedEvent += new NewBitmapReceivedCBType(aNewBitmapReceivedCB);
        }
       
        //Handles the reception of 1 UDP packet of data.
        public void ReceivedHandler(int NumberOfAvailableData)
        {                       
            int PacketSize = Packet.GetHeaderSize() + Packet.DEFAULT_PACKET_SIZE;

            if (NumberOfAvailableData >= PacketSize)
            {
               
                while (aUDPListener.GetNumberOfReceivedData() >= PacketSize)
                {
                    byte[] buffer = new byte[PacketSize];

                    aUDPListener.ReceiveData(ref buffer, PacketSize);

                    Packet p = new Packet(buffer);

                    PacketReconstructors.ReconstructFile(p);
                   
                }
                
            }
            

        }

        public void PacketReconstructedCBHandler(int FileID, byte[] filebyte, int bytesRead)
        {   
            CodecUtility aCodec = new CodecUtility();
            SetLatestFrame(aCodec.DecompressJPEGArrayToBpm(filebyte));

            //Call the callback
            if (aNewBitmapReceivedEvent != null)
            {
                aNewBitmapReceivedEvent(GetLatestFrame());
            }
            
        }

        public override void Start()
        {

            if (State == CameraState.CAMERA_STOPPED)
            {
                FrameNumber = 0;

                aUDPSender.SendStringNow("<C" + ID + "O>");

                State = CameraState.CAMERA_STARTED;
            }
        }

        public override void Stop()
        {
            if (State == CameraState.CAMERA_STARTED)
            {             
            
                ResetFrameNumber();

                aUDPSender.SendStringNow("<C" + ID + "F>");
                
                State = CameraState.CAMERA_STOPPED;
            }

        }
    }
}
