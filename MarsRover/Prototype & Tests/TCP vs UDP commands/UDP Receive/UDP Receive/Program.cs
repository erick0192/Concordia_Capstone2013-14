using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UDP_Receive
{
    /// <summary>
    /// This UDP server receives a fake command. The fake command is sent from the UDP client at fixed intervals. The value of those intervals are held within the commmand.
    /// </summary>
    class Program
    {
        static UdpClient udpServer;
        static int packets_received;
        static int timerInterval;
        static IPEndPoint udpEp;
        static bool receiveThreadContinue;
        static int commandSize;
        static void Main(string[] args)
        {
            int port = 5000;
            if (args.Length != 1 || (!(int.TryParse(args[0], out port))) )
            {
                Console.WriteLine("What're you doing? These are the arguments that need be passed:");
                Console.WriteLine("1. Port (int)\n");
                Console.WriteLine("Press enter to exit and try again (don't give up!)");
                Console.ReadLine();

                return;
            }

            
            receiveThreadContinue = true;
            
            udpEp = new IPEndPoint(IPAddress.Any, port);

            
            
            //----------------------------------
            udpServer = new UdpClient(port);

            //----------------------------------
            Thread receiveThread = new Thread(new ThreadStart(receiver));
            receiveThread.Start();

            ///----------------------------------

            Console.WriteLine("Press Enter to stop and see some stats\n");
            Console.ReadLine();
            receiveThreadContinue = false;

            Console.WriteLine("Packets received: {0}\nTimerInterval: {1}\nCommand size: {2}", packets_received, timerInterval, commandSize);
            Console.WriteLine("Press Enter to exit\n");
            Console.ReadLine();
            return;
        }

        static void receiver()
        {
            byte[] receivedData = new byte[1000];
            while (receiveThreadContinue == true)
            {
                receivedData = udpServer.Receive(ref udpEp);
                commandSize = receivedData.Length;
                timerInterval = BitConverter.ToInt32(receivedData, 0);
                packets_received++;
            }
        }
    }
}
