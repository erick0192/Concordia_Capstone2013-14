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

    public class UDPListenerCameraDevice : RemoteCameraDevice
    {
      
        public delegate void NewBitmapReceivedCBType(Bitmap aBitmap);
        public event NewBitmapReceivedCBType aNewBitmapReceivedEvent;

        private UDPListener aUDPListener;
        private PacketReconstructor PacketReconstructors;
        private TypeConverter ImageConverter;
        
        private RemoteUDPStatistics aUDPStatistics;
        
        public UDPListenerCameraDevice(string IpAddress, int Port)
        {
            PacketReconstructors = new PacketReconstructor(new Packet().GetBytes().Length, PacketReconstructedCBHandler);
            aUDPListener = new UDPListener(Port, ReceivedHandler);
            aUDPStatistics = new RemoteUDPStatistics(aUDPListener, 1000);

            ImageConverter = TypeDescriptor.GetConverter(typeof(Bitmap));

        }

        public void RegisterListener( NewBitmapReceivedCBType aNewBitmapReceivedCB)
        {
            aNewBitmapReceivedEvent += new NewBitmapReceivedCBType(aNewBitmapReceivedCB);
        }
       
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
            LatestFrame = aCodec.DecompressJPEGArrayToBpm(filebyte);
                               
            //Call the callback
            if (aNewBitmapReceivedEvent != null)
            {
                aNewBitmapReceivedEvent(LatestFrame);
            }

            PacketReconstructors.GetReconstructedPackets().Clear();

        }

    
    }
}
