using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Net;
using System.Net.Sockets;

namespace UDP_Send
{
    /// <summary>
    /// This UDP client sends a frame containing a pseudo-command to a UDP server. The command is sent using a timer. The command contains the value of the timer, not an actual command. 
    /// </summary>
    class Program
    {
        static byte[] command;
        static int packet_sent_count;
        static UdpClient udpClient;

        static void Main(string[] args)
        {
            int timerInterval;
            int commandSize;
            string serverIP;
            int port;

            if (args.Length != 4)
            {
                Console.WriteLine("What're you doing? These are the arguments that need be passed:");
                Console.WriteLine("1. Timer interval (int)");
                Console.WriteLine("2. Command size in bytes (int)");
                Console.WriteLine("3. IP address of the server (string)");
                Console.WriteLine("4. Port to connect to (int)");
                Console.ReadLine();
                return;
            }
            else
            {
                timerInterval = int.Parse(args[0]);
                commandSize = int.Parse(args[1]);
                serverIP = args[2];
                port = int.Parse(args[3]);
            }
            

            //keeping track of how many commands over UDP have been sent
            packet_sent_count = 0;

            //set the command size in terms of bytes
            command = new byte[commandSize];

            //filling up the command with the value of the timer interval instead of an actual command;
            //good way to tell the receiving end what our interval is on this side
            Array.Copy(BitConverter.GetBytes(timerInterval), command, BitConverter.GetBytes(commandSize).Length);

            //---------------------
            //setting up the timer
            Timer tim = new Timer((double)timerInterval);
            tim.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            tim.Start();
            //---------------------
            //setting up UDP client; UDP client sends to server
            udpClient = new UdpClient(serverIP, port);

            //---------------------

            Console.WriteLine("Press enter to stop the sending and see some stats\n");
            Console.ReadLine();
            tim.Stop();
            Console.WriteLine("Packets sent:{0}\nTimer interval (ms):{1}\nCommand size (bytes):{2}\n", packet_sent_count, timerInterval, commandSize);


            Console.WriteLine("Press enter again to exit");
            Console.ReadLine();

        }

        static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //keep counting how many packets are sent
            packet_sent_count++;
            
            //send the ducking thing
            udpClient.Send(command, command.Length);
        }

        
    }
}
