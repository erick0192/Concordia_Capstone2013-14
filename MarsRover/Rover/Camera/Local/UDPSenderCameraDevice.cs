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

namespace Rover
{
    public class UDPSenderCameraDevice : LocalCameraDevice
    {
        private UDPSender aUDPSender;        
        private ImageConverter converter;
        private Stopwatch stopwatch;
        private CodecUtility aCodecUtility;

        private LocalUDPStatistics aUDPStatistics;
        
        public UDPSenderCameraDevice(string IpAddress, int Port, string aCameraName, string aMonikerString, int aCameraID) 
            : base(aCameraName, aMonikerString, aCameraID)
        {
            
            //register callback on BitmapAcquired
            RegisterBitmapAcquiredCB(BitmapAcquiredCBHandler);
            
            //Create a socket connection for data to go out.
            aUDPSender = new UDPSender(IpAddress, Port);                            
            stopwatch = new Stopwatch();
            converter = new ImageConverter();
            aUDPStatistics = new LocalUDPStatistics(aUDPSender, 1000);     
            aCodecUtility = new CodecUtility();
        }

        public void BitmapAcquiredCBHandler(Bitmap aNewBitmap)
        {
            stopwatch.Restart();            

            //Console.WriteLine("BitmapAcquiredCBHandler:");
            //Console.WriteLine("From CameraId " + GetID());
            byte[] newBA = aCodecUtility.CompressBmpToJPEGArray(aNewBitmap);
            //byte[] newBA = (byte[])converter.ConvertTo(aNewBitmap, typeof(byte[]));

            //Total size of the packet to send including header  +  data.
            PacketPartitionner pp = new PacketPartitionner(GetID(), newBA.Length, Packet.GetHeaderSize() + Packet.DEFAULT_PACKET_SIZE, null);
            pp.PartitionFile(newBA, newBA.Length);

            //Console.WriteLine("Bitmap of size:" + newBA.Length + ", Splitted in:" + pp.GetPartitionnedPackets().Count + "Packets of " + (Packet.GetHeaderSize() + Packet.DEFAULT_PACKET_SIZE) + " Bytes");
                                
            //Remove prints to the console
            //This is a critical section, we don't to have more than 1 camera sending data at the same time.
            for (int i = 0; i < pp.GetPartitionnedPackets().Count; i++)
            {
                //Console.WriteLine("Send packet number:" + i + "/" + (pp.GetPartitionnedPackets().Count - 1));

                //((Packet)pp.GetPartitionnedPackets()[i]).PrintInfo();

                Packet p = ((Packet)(pp.GetPartitionnedPackets()[i]));

                byte[] SerializedPacket = p.GetBytes();

                //aUDPSender.SendDataUDP(SerializedPacket, SerializedPacket.Length);
                aUDPSender.SendNow(SerializedPacket, SerializedPacket.Length);

                Thread.Sleep(10);
            }

            //Thread.Sleep(100);

            //clear the array list so you dont send alway the same image.
            //pp.GetPartitionnedPackets().Clear();
                   
            stopwatch.Stop();
            // Write result
            //Console.WriteLine("Time elapsed ms: {0}", stopwatch.ElapsedMilliseconds);

        }

    }
}
