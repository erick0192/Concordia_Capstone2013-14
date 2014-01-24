using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rover
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Header
    {
        public int FileID;
        public int FileSize;
        public int PacketNumber;
        public int TotalNbPackets;
        public int DataUsed;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Frame
    {
        public Header aHeader;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Packet.DEFAULT_PACKET_SIZE)]
        public byte[] Data;

    }


    public class Packet
    {
        public const int DEFAULT_PACKET_SIZE = 16 * 1024;

        public Frame aFrame;

        public Packet()
        {
            aFrame.aHeader.FileID = 0;
            aFrame.aHeader.FileSize = 0;
            aFrame.aHeader.PacketNumber = 0;
            aFrame.aHeader.TotalNbPackets = 0;
            aFrame.aHeader.DataUsed = 0;
            aFrame.Data = new byte[DEFAULT_PACKET_SIZE];
        }

        /// <summary>
        /// Create a packet object, with related file id, and file size.
        /// </summary>
        /// <param name="FileID"></param>
        /// <param name="FileSize"></param>
        /// <param name="PacketNumber"></param>
        /// <param name="TotalNbPackets"></param>
        /// <param name="DataUsed"></param>
        /// <param name="Data"></param>
        public Packet(int FileID, int FileSize, int PacketNumber, int TotalNbPackets, int DataUsed, byte[] Data)
        {
            aFrame.aHeader.FileID = FileID;
            aFrame.aHeader.FileSize = FileSize;
            aFrame.aHeader.PacketNumber = PacketNumber;
            aFrame.aHeader.TotalNbPackets = TotalNbPackets;
            aFrame.aHeader.DataUsed = DataUsed;

            aFrame.Data = new byte[Data.Length];
            Data.CopyTo(aFrame.Data, 0);
            //aFrame.Data = Data;       
        }

        public Packet(byte[] packet)
        {
            GCHandle pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            aFrame = (Frame)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(Frame));
            pinnedPacket.Free();
        }


        public byte[] GetBytes()
        {
            int size;
            unsafe
            {
                size = aFrame.Data.Length + sizeof(Header);
            }

            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(aFrame, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public void PrintInfo()
        {
            Console.WriteLine("FileID:" + aFrame.aHeader.FileID);
            Console.WriteLine("FileSz:" + aFrame.aHeader.FileSize);
            Console.WriteLine("PktNb:" + aFrame.aHeader.PacketNumber);
            Console.WriteLine("TotalNbPkt:" + aFrame.aHeader.TotalNbPackets);
            Console.WriteLine("DataUsed:" + aFrame.aHeader.DataUsed);
        }

        static public int GetHeaderSize()
        {
            Header aHeader = new Header();

            int size = Marshal.SizeOf(aHeader);

            return size;
        }

    }

}
