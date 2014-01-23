using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rover
{
    public class UDPListener
    {
        public delegate void ReceivedDataCBType(int NbAvailableData);
        private event ReceivedDataCBType ReceivedDataCB;

        #region Attributes
        private int port;
       // private Logger logger;

        private UdpClient listener;
        private IPEndPoint groupEP;
      
        //private Queue<byte> ReceptionQueue;
        private Object ReceptionQueueLock = new Object();
        private ConcurrentQueue<byte> ReceivingQueue;
        
        private int TotalNbDataReceived;
        #endregion


        public UDPListener(int aPort, ReceivedDataCBType aReceivedDataCB)
        {
            port = aPort;
            
            listener = new UdpClient(port);
            groupEP = new IPEndPoint(IPAddress.Any, port);

            //logger = NLog.LogManager.GetCurrentClassLogger();
            
           // ReceptionQueue = new Queue<byte>(16*1024*10);
            ReceivingQueue = new ConcurrentQueue<byte>();

            ReceivedDataCB += aReceivedDataCB;

            Initialize();
        }
        
        public void Initialize()
        {
            Thread t = new Thread(StartListening);
            t.IsBackground = false;
            t.Priority = ThreadPriority.Highest;
            t.Start();
        }

        public int GetTotalNbDataINOUT()
        {
            return TotalNbDataReceived;
        }

        public int GetNumberOfReceivedData()
        {
            return ReceivingQueue.Count;

        }
        
        public int  ReceiveData( ref byte[] data, int size )
        {
            int NbOfDataToRead = Math.Min(size, ReceivingQueue.Count);
          
            if (NbOfDataToRead > 0)
            {
                for (int i = 0; i < NbOfDataToRead; i++)
                {
                    ReceivingQueue.TryDequeue(out data[i]);
                }
            }

            return NbOfDataToRead;
          
        }

        private void StartListening()
        {
          
            try
            {
                while (true)
                {
                 //   logger.Trace("Starting UDPListener on port " + port);
                    
                    //BLOCKING CALL                    
                    byte[] bytes = listener.Receive(ref groupEP); 
                
                    //Console.WriteLine("Receive data:" + bytes.Length);                    
                    TotalNbDataReceived += bytes.Length;


                   
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        ReceivingQueue.Enqueue(bytes[i]);                        
                    }
                   

                    //string message = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    //logger.Trace("Received broadcast from " + groupEP.ToString() + ". Message: " + message);                    
                    if(ReceivedDataCB != null)
                    {
                        //return total number of byte received  so far
                        ReceivedDataCB(ReceivingQueue.Count);
                    }
                }

            }
            catch (Exception e)
            {
               // logger.Debug(e.ToString());
                Console.WriteLine(e.ToString());
            }
        }
    }
}
