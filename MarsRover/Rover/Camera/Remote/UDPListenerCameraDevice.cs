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


namespace Rover
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

            //Thread t = new Thread(ReceiveData);
            //t.IsBackground = false;
            //t.Priority = ThreadPriority.Highest;
            //t.Start();

        }

        public void RegisterListener( NewBitmapReceivedCBType aNewBitmapReceivedCB)
        {
            aNewBitmapReceivedEvent += new NewBitmapReceivedCBType(aNewBitmapReceivedCB);
        }
        /*
        public void ReceiveData()
        {

            int PacketSize = Packet.GetHeaderSize() + Packet.DEFAULT_PACKET_SIZE;
            while (true)
            {
                while (aUDPListener.GetNumberOfReceivedData() >= PacketSize)
                {
                    byte[] buffer = new byte[PacketSize];

                    aUDPListener.ReceiveData(ref buffer, PacketSize);

                    Packet p = new Packet(buffer);

                    PacketReconstructors.ReconstructFile(p);

                    // Console.WriteLine("Receive packet number:" + p.aFrame.aHeader.PacketNumber + "/" + (p.aFrame.aHeader.TotalNbPackets - 1));

                    // p.PrintInfo();
                }
            }
        }
        */
        public void ReceivedHandler(int NumberOfAvailableData)
        {           

            //Console.WriteLine(NumberOfAvailableData + " bytes received");
            int PacketSize = Packet.GetHeaderSize() + Packet.DEFAULT_PACKET_SIZE;

            if (NumberOfAvailableData >= PacketSize)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (aUDPListener.GetNumberOfReceivedData() >= PacketSize)
                {
                    byte[] buffer = new byte[PacketSize];

                    aUDPListener.ReceiveData(ref buffer, PacketSize);

                    Packet p = new Packet(buffer);

                    PacketReconstructors.ReconstructFile(p);

                   // Console.WriteLine("Receive packet number:" + p.aFrame.aHeader.PacketNumber + "/" + (p.aFrame.aHeader.TotalNbPackets - 1));

                   // p.PrintInfo();
                }
                

                stopwatch.Stop();
                // Write result
                //Console.WriteLine("Time elapsed ms: {0}", stopwatch.ElapsedMilliseconds);
            }
            

        }

        public void PacketReconstructedCBHandler(int FileID, byte[] filebyte, int bytesRead)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CodecUtility aCodec = new CodecUtility();
            LatestFrame = aCodec.DecompressJPEGArrayToBpm(filebyte);
                               
            //Call the callback
            if (aNewBitmapReceivedEvent != null)
            {
                aNewBitmapReceivedEvent(LatestFrame);
            }

            PacketReconstructors.GetReconstructedPackets().Clear();

            stopwatch.Stop();
            // Write result
            //Console.WriteLine("Time elapsed ms: {0}", stopwatch.ElapsedMilliseconds);
        }

    
    }
}
