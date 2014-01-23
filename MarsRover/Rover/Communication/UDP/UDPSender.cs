using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Rover
{
    public class UDPSender
    {
        #region Attributes

        #endregion

        #region Singleton Constructor/Properties

        //Critical area of the constructor is locked so that only one instance is created even in a multithreaded environment
        //http://msdn.microsoft.com/en-us/library/ff650316.aspx
        //private static volatile UDPSender instance;
        //private static object syncRoot = new Object();
        private Socket s;
        private IPAddress broadcast;
        private IPEndPoint ep;
        private int TotalNbDataSent;
        private ConcurrentQueue<byte> cq;

        public UDPSender (string IPAdress, int Port)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            broadcast = IPAddress.Parse(IPAdress);
            ep = new IPEndPoint(broadcast, Port);
            cq = new ConcurrentQueue<byte>();

           // Initialize();
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            Thread t = new Thread(SendDataProcess);
            t.IsBackground = false;
            t.Priority = ThreadPriority.Highest;
            t.Start();
        }

        public int GetTotalNbDataINOUT()
        {
            return TotalNbDataSent;
        }

        public void SendNow(byte[] Data, int Length)
        {
            TotalNbDataSent += Length;

            s.SendTo(Data, ep);
        }

        public void SendDataProcess()
        {
            try
            {
                while (true)
                {
                    int NbOfDataToSend = Math.Min(1024, cq.Count);

                    if (NbOfDataToSend > 0)
                    {
                      
                        byte[] data = new byte[NbOfDataToSend];

                        for (int i = 0; i < NbOfDataToSend; i++)
                        {
                            cq.TryDequeue(out data[i]);
                        }

                        s.SendTo(data, ep);
                        
                    }

                  
                }

            }
            catch (Exception e)
            {
                // logger.Debug(e.ToString());
                Console.WriteLine(e.ToString());
            }
        }

        public void SendDataUDP(byte[] Data, int Length)
        {
            TotalNbDataSent += Length;

            for (int i = 0; i < Length; i++)
            {
                cq.Enqueue(Data[i]);
            }
        }

        #endregion
    }
}
