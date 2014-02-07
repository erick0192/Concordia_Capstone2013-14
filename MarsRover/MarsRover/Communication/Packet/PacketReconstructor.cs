using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover
{


    class PacketReconstructor
    {
        public delegate void PacketReconstructedCBType(int FileID, byte[] FullFile, int FileSize);
        private event PacketReconstructedCBType aPacketReconstructedCBHandler;

        private byte[] d;
        private int PacketSize;

        ArrayList ReconstructedPackets = new ArrayList();

        /// <summary>
        /// Constructor of the packetReconstructor class, remember the packet size, and a callback function.
        /// </summary>
        /// <param name="PacketSize"></param>
        /// <param name="aPacketReconstructedCBHandler"></param>
        public PacketReconstructor(int PacketSize, PacketReconstructedCBType aPacketReconstructedCBHandler)
        {
            this.PacketSize = PacketSize;
            this.aPacketReconstructedCBHandler += new PacketReconstructedCBType(aPacketReconstructedCBHandler);

        }

        /// <summary>
        /// Return all the packets received
        /// </summary>
        /// <returns></returns>
        public ArrayList GetReconstructedPackets()
        {
            return ReconstructedPackets;
        }

        /// <summary>
        /// Reconstruct a file packet by packet, when the last packet is receive the file is created.
        /// </summary>
        /// <param name="newPacket"></param>
        public void ReconstructFile(Packet newPacket)
        {
            ReconstructedPackets.Add(newPacket);
          
            //Console.WriteLine("Reconstruct Packet ID " + newPacket.aFrame.aHeader.FileID);
          
            //last packet, and we received all the packets.
            if (newPacket.aFrame.aHeader.PacketNumber == newPacket.aFrame.aHeader.TotalNbPackets - 1 &&
                ReconstructedPackets.Count >= newPacket.aFrame.aHeader.TotalNbPackets)
            {
               // Console.WriteLine("Number of packet received:" + ReconstructedPackets.Count);
                //Sanity check on packet 
                for (int i = 0; i < newPacket.aFrame.aHeader.TotalNbPackets; i++)
                {
                    if (((Packet)ReconstructedPackets[i]).aFrame.aHeader.FileID != newPacket.aFrame.aHeader.FileID)
                    {
                        ReconstructedPackets.Clear();
                        
                        return;
                    }
                }

                d = new byte[newPacket.aFrame.aHeader.TotalNbPackets * Packet.DEFAULT_PACKET_SIZE];
                
                for (int i = 0; i < newPacket.aFrame.aHeader.TotalNbPackets; i++)
                {
                    Buffer.BlockCopy(((Packet)ReconstructedPackets[i]).aFrame.Data,
                                        0,
                                        d,
                                        i * Packet.DEFAULT_PACKET_SIZE,
                                        ((Packet)ReconstructedPackets[i]).aFrame.aHeader.DataUsed);

                }

                if (aPacketReconstructedCBHandler != null)
                {
                    aPacketReconstructedCBHandler(newPacket.aFrame.aHeader.FileID, d, d.Length);
                }

                d = null;
                ReconstructedPackets.Clear();

            }
        }
    }

}
