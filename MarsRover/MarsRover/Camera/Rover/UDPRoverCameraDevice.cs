using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace MarsRover
{
    public class UDPRoverCameraDevice : RoverCameraDevice
    {
        private UDPSender aUDPSender;        
        private ImageConverter converter;        
        private CodecUtility aCodecUtility;
        private long aImageQuality;
        private LocalUDPStatistics aUDPStatistics;
        private Random aRandomGenerator;

        public UDPRoverCameraDevice(string IpAddress, int Port, string aCameraName, string aMonikerString, int aCameraID, long ImageQuality, int FrameRateDivider)
            : base(aCameraName, aMonikerString, aCameraID, FrameRateDivider)
        {
            aImageQuality = ImageQuality;

            //register callback on BitmapAcquired
            RegisterBitmapAcquiredCB(BitmapAcquiredCBHandler);
            
            //Create a socket connection for data to go out.
            aUDPSender = new UDPSender(IpAddress, Port);                                        
            converter = new ImageConverter();
            aUDPStatistics = new LocalUDPStatistics(aUDPSender, 1000);     
            aCodecUtility = new CodecUtility();
            aRandomGenerator = new Random();
        }
        
        public void BitmapAcquiredCBHandler(Bitmap aNewBitmap)
        {
            byte[] newBA = aCodecUtility.CompressBmpToJPEGArray(aImageQuality, aNewBitmap);
            int FileID = aRandomGenerator.Next();
           
            //Total size of the packet to send including header  +  data.
            PacketPartitionner pp = new PacketPartitionner(FileID, newBA.Length, Packet.GetHeaderSize() + Packet.DEFAULT_PACKET_SIZE, null);
            pp.PartitionFile(newBA, newBA.Length);

            //Console.WriteLine("Partition File ID " + FileID + " In " + pp.GetPartitionnedPackets().Count + " Packets");
            
            //Remove prints to the console            
            for (int i = 0; i < pp.GetPartitionnedPackets().Count; i++)
            {                
                Packet p = ((Packet)(pp.GetPartitionnedPackets()[i]));
                
                //Console.WriteLine("Send Packet:"+i);
            
                byte[] SerializedPacket = p.GetBytes();

                //Sending the data to a blocking queue that gets process by a thread generate additionnal delay
                //The best solution to reduce the lag for now is to send the data directly with SendNow method.
                //aUDPSender.SendDataUDP(SerializedPacket, SerializedPacket.Length);
             
                aUDPSender.SendBytesNow(SerializedPacket, SerializedPacket.Length);
                
            }

            pp.GetPartitionnedPackets().Clear();         
        }

    }
}
