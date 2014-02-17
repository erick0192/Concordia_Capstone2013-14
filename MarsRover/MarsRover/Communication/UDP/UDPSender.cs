using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MarsRover
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
        //private Thread SendingThread;

        public UDPSender (string IPAdress, int Port)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            broadcast = IPAddress.Parse(IPAdress);
            ep = new IPEndPoint(broadcast, Port);
            cq = new ConcurrentQueue<byte>();
            
            s.SendBufferSize = 65535;
            /*SendingThread = new Thread(SendDataProcess);
            SendingThread.IsBackground = false;
            SendingThread.Priority = ThreadPriority.Highest;
            SendingThread.Start();*/
        }
        #endregion

        #region Methods

        public int GetTotalNbDataINOUT()
        {
            return TotalNbDataSent;
        }

        public bool SendBytesNow(byte[] Data, int Length)
        {
            if (Length > s.SendBufferSize)
                return false;

            TotalNbDataSent += Length;
            s.SendTo(Data, ep);

            return true;
        }

        public void SendStringNow(string command)
        {
            byte[] sendbuf = Encoding.ASCII.GetBytes(command);

            TotalNbDataSent += sendbuf.Length;           

            s.SendTo(sendbuf, ep);
        }

        /*public void SendDataInQueue(byte[] Data, int Length)
        {
            TotalNbDataSent += Length;

            for (int i = 0; i < Length; i++)
            {
                cq.Enqueue(Data[i]);
            }
        }

        private void SendDataProcess()
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
        }*/
        #endregion
    }
}
