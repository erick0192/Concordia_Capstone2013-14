using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using System.Net.Sockets;
using System.Net;

namespace MarsRover.Communication
{
    public sealed class Pinger
    {
        #region Properties
        private string roverIPAddress;
        public string RoverIPAddress
        {
            get
            {
                return roverIPAddress;
            }
            set
            {
                roverIPAddress = value;
            }
        }

        private long RTT;
        public long PingRTT
        {
            get { return RTT; }
            set
            {
                RTT = value;
                if (RTTChanged != null) RTTChanged(value);
            }
        }

        private bool connectedToRover;
        public bool ConnectedToRover
        {
            get { return connectedToRover; }
            set
            {
                connectedToRover = value;
                if (ConnectivityChanged != null) ConnectivityChanged(value);
            }
        }

        #endregion

        #region Event/Delegates

        public delegate void OnRTTChanged(long RTT);
        public event OnRTTChanged RTTChanged;

        public delegate void OnConnectivityChanged(bool connectedToRover);
        public event OnConnectivityChanged ConnectivityChanged;

        #endregion

        #region Singleton Constructor/Properties

        //Critical area of the constructor is locked so that only one instance is created even in a multithreaded environment
        //http://msdn.microsoft.com/en-us/library/ff650316.aspx
        private static volatile Pinger instance;
        private static object syncRoot = new Object();

        private Pinger()
        {
            Thread t = new Thread(() => Ping());
            t.IsBackground = true;
            //t.Start();
        }

        public static Pinger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Pinger();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Methods

        private void Ping()
        {
            byte[] data = new byte[1024];
            int recv;
            Socket host = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(roverIPAddress), 0); //ICMP doesn't use port numbers
            EndPoint ep = (EndPoint)iep;
            ICMP packet = new ICMP();
            packet.Type = 0x08;
            packet.Code = 0x00;
            packet.Checksum = 0;
            Buffer.BlockCopy(BitConverter.GetBytes((short)1), 0, packet.Message, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((short)1), 0, packet.Message, 2, 2);
            data = Encoding.ASCII.GetBytes("ping");
            Buffer.BlockCopy(data, 0, packet.Message, 4, data.Length);
            packet.MessageSize = data.Length + 4;
            int packetsize = packet.MessageSize + 4;
            UInt16 chcksum = packet.getChecksum();
            packet.Checksum = chcksum;
            host.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);

            while (true)
            {
                DateTime sentAt = DateTime.Now;
                DateTime recvdAt = sentAt;
                host.SendTo(packet.getBytes(), packetsize, SocketFlags.None, iep);
                try
                {
                    data = new byte[1024];
                    recv = host.ReceiveFrom(data, ref ep);
                    recvdAt = DateTime.Now;
                    ICMP response = new ICMP(data, recv);
                    DateTime recvd = DateTime.Now;
                    ConnectedToRover = true;
                    PingRTT = (long)(recvdAt - sentAt).TotalMilliseconds;
                }
                catch (SocketException)
                {
                    ConnectedToRover = false;
                    PingRTT = 0;
                }

                Thread.Sleep(500);
            }
        }

        #endregion


        //From "C# Network Programming" by Richard Blum
        class ICMP
        {
            public byte Type;
            public byte Code;
            public UInt16 Checksum;
            public int MessageSize;
            public byte[] Message = new byte[1024];

            public ICMP() { }

            public ICMP(byte[] data, int size)
            {
                Type = data[20];
                Code = data[21];
                Checksum = BitConverter.ToUInt16(data, 22);
                MessageSize = size - 24;
                Buffer.BlockCopy(data, 24, Message, 0, MessageSize);
            }

            public byte[] getBytes()
            {
                byte[] data = new byte[MessageSize + 9];
                Buffer.BlockCopy(BitConverter.GetBytes(Type), 0, data, 0, 1);
                Buffer.BlockCopy(BitConverter.GetBytes(Code), 0, data, 1, 1);
                Buffer.BlockCopy(BitConverter.GetBytes(Checksum), 0, data, 2, 2);
                Buffer.BlockCopy(Message, 0, data, 4, MessageSize);
                return data;
            }

            public UInt16 getChecksum()
            {
                UInt32 chcksm = 0;
                byte[] data = getBytes();
                int packetsize = MessageSize + 8;
                int index = 0;

                while (index < packetsize)
                {
                    chcksm += Convert.ToUInt32(BitConverter.ToUInt16(data, index));
                    index += 2;
                }
                chcksm = (chcksm >> 16) + (chcksm & 0xffff);
                chcksm += (chcksm >> 16);
                return (UInt16)(~chcksm);
            }
        }


    }
}
