using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rover
{
    
  
    class PacketPartitionner
    {
        public delegate void PacketPartitionnerCBType(ArrayList PartitionnedPacketList);
        private event PacketPartitionnerCBType PacketPartitionnerCBHandler;
        
        public int FileID;
        public int FileSize;
        int PacketSizeWHeader;     

        ArrayList PartitionnedPackets = new ArrayList();

        /// <summary>
        /// Constructor of the PacketParitionner class, refers to a file id, file size, and packet size.
        /// </summary>
        /// <param name="FileID"></param>
        /// <param name="FileSize"></param>
        /// <param name="PacketSize"></param>
        public PacketPartitionner(int FileID, int FileSize, int PacketSizeWHeader, PacketPartitionnerCBType PacketPartitionnerCBHandler)
        {
            this.FileID = FileID;
            this.FileSize = FileSize;
            this.PacketSizeWHeader = PacketSizeWHeader;

            if (PacketPartitionnerCBHandler != null)
            { this.PacketPartitionnerCBHandler += new PacketPartitionnerCBType(PacketPartitionnerCBHandler); }
        }

        /// <summary>
        /// Return the Partitionned packets stored in the array list
        /// </summary>
        /// <returns></returns>
        public ArrayList GetPartitionnedPackets()
        {
            return PartitionnedPackets;
        }
                
        /// <summary>
        /// Partition a file in packets and adds the packet to the arraylist.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="FileSize"></param>
        public void PartitionFile(byte[] Data, int FileSize)
        {
            //Clear previous packets in the arraylist
            PartitionnedPackets.Clear();

            //Calculate the number of full packets
            int NbFullPkt = FileSize / Packet.DEFAULT_PACKET_SIZE;
            int NbRemainingPkt = FileSize % Packet.DEFAULT_PACKET_SIZE;
            int TotalNbOfPackets;

            if(NbRemainingPkt > 0)
            {
                TotalNbOfPackets = NbFullPkt + 1;
            }
            else
            {
                TotalNbOfPackets = NbFullPkt;
            }

            
            for (int i = 0; i < NbFullPkt; i++)
            {
                byte[] d = new byte[Packet.DEFAULT_PACKET_SIZE];

                Buffer.BlockCopy(Data, i * Packet.DEFAULT_PACKET_SIZE, d, 0, Packet.DEFAULT_PACKET_SIZE);

                Packet p = new Packet(FileID, FileSize, i, TotalNbOfPackets, Packet.DEFAULT_PACKET_SIZE, d);
                
                PartitionnedPackets.Add(p);

                d = null;
            }

            if (NbRemainingPkt > 0)
            {
                byte[] d = new byte[Packet.DEFAULT_PACKET_SIZE];

                Buffer.BlockCopy(Data, NbFullPkt * Packet.DEFAULT_PACKET_SIZE, d, 0, NbRemainingPkt);

                Packet p = new Packet(FileID, FileSize, TotalNbOfPackets-1, TotalNbOfPackets, NbRemainingPkt, d);

                PartitionnedPackets.Add(p);

                d = null;
            }

            if (PacketPartitionnerCBHandler != null)
            {
                PacketPartitionnerCBHandler(PartitionnedPackets);
            }

        }
         
        
    }
}
