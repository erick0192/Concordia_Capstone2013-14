using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rover
{
    public class TCPClient
    {
        public delegate void TransmittedDataCBType();
        public delegate void ReceivedDataCDType(TCPClient aClient, int NbAvailableData);

        const int QUEUE_SIZE_MAX = 1024;

        private TransmittedDataCBType aTransmittedDataCBHandler;
        private ReceivedDataCDType aReceivedDataCDHandler;
       
        private Thread TransmissionThread;
        private Thread ReceptionThread;
       
        Queue<byte> TransmissionQueue;
        Queue<byte> ReceptionQueue;
        
        private Object TransmissionQueueLock = new Object();
        private Object ReceptionQueueLock = new Object();

        NetworkStream clientStream;
        IPEndPoint serverEndPoint;

        /// <summary>
        /// Create a TCP client objet, on the specified ip address and port.
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <param name="Port"></param>
        /// <param name="aTransmittedDataCB"></param>
        /// <param name="aReceivedDataCD"></param>
        public TCPClient(String IPAddressStr, int Port, TransmittedDataCBType aTransmittedDataCB, ReceivedDataCDType aReceivedDataCD)
        {
            TcpClient aClient = new TcpClient();

            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(IPAddressStr), Port);
       
            aClient.Connect(serverEndPoint);

            clientStream = aClient.GetStream();

            Init(QUEUE_SIZE_MAX, aTransmittedDataCB, aReceivedDataCD);
           
        }

        /// <summary>
        /// Internal init function
        /// </summary>
        /// <param name="QueueSize"></param>
        /// <param name="aTransmittedDataCB"></param>
        /// <param name="aReceivedDataCD"></param>
        public void Init(int QueueSize, TransmittedDataCBType aTransmittedDataCB, ReceivedDataCDType aReceivedDataCD)
        {

            aReceivedDataCDHandler = aReceivedDataCD;
            aTransmittedDataCBHandler = aTransmittedDataCB;

            TransmissionQueue = new Queue<byte>(QueueSize);
            ReceptionQueue = new Queue<byte>(QueueSize);

            TransmissionThread = new Thread(new ThreadStart(TransmissionTask));
            TransmissionThread.Start();

            ReceptionThread = new Thread(new ThreadStart(ReceptionTask));
            ReceptionThread.Start();


        }

        public int GetNumberOfReceivedData()
        {
            return ReceptionQueue.Count;

        }
        /// <summary>
        /// Send data to the conneted server
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public void SendData(byte[] data, int size)
        {
            for (int i = 0; i < size; i++)
            {
                lock (TransmissionQueueLock)
                {
                    TransmissionQueue.Enqueue(data[i]);
                }
            }
        }

        /// <summary>
        /// Receive data from the connected server.        
        /// </summary>
        /// <param name="data"></param> Buffer where to copy the data
        /// <param name="size"></param> Size of the buffer.
        /// <returns>Returns 0 if no data is available , otherise returns the number of byte read</returns>
        public int ReceiveData(ref byte[] data, int size)
        {
            
            int NbOfDataToRead = Math.Min(size, ReceptionQueue.Count);

            if (NbOfDataToRead > 0)
            {
                for (int i = 0; i < NbOfDataToRead; i++)
                {
                    lock (ReceptionQueueLock)
                    {
                        data[i] = ReceptionQueue.Dequeue();
                    }
                }

                return NbOfDataToRead;

            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Transmission task
        /// </summary>
        private void TransmissionTask()
        {
            while (true)
            {
                if (TransmissionQueue.Count > 0)
                {
                    lock (TransmissionQueueLock)
                    {
                        clientStream.WriteByte(TransmissionQueue.Dequeue());
                    }

                    if (TransmissionQueue.Count == 0)
                    {
                        aTransmittedDataCBHandler();
                    }
                }
                
            }
        }

        /// <summary>
        /// Reception Task
        /// </summary>
        private void ReceptionTask()
        {
            while (true)
            {
                if (clientStream.DataAvailable)
                {                    
                    int data = clientStream.ReadByte();

                    if (data == -1)
                    {
                        //End of stream
                        break;
                    }
                    else
                    {                       
                        lock (ReceptionQueueLock)
                        {
                            ReceptionQueue.Enqueue((byte)data);
                        }

                        
                    }

                    //no more data received.
                    if (clientStream.DataAvailable == false)
                    {
                        aReceivedDataCDHandler(this, ReceptionQueue.Count);
                    }
                }

            }

        }
    }
}
