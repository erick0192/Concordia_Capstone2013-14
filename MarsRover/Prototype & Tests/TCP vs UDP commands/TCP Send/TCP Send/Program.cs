using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TCP_Send
{
    class Program
    {
        static byte[] command;
        static string serverIP;
        static int port;
        static int packet_sent_count;
        static int commandSize;
        static int timerInterval;
        static TcpClient tcpClient;
        static Stream tcpStream;

        static void Main(string[] args)
        {
            packet_sent_count = 0;

            if (args.Length != 4)
            {
                Console.WriteLine("What're you doing? These are the arguments that need be passed:");
                Console.WriteLine("1. Timer interval (int)");
                Console.WriteLine("2. Command size in bytes (int) (not greater than 256 bytes plz)");
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

            //preparing command 
            command = new byte[commandSize];
            Array.Copy(BitConverter.GetBytes(timerInterval), command, BitConverter.GetBytes(commandSize).Length);

            //set up TCP connection
            tcpClient = new TcpClient(serverIP, port);
            tcpStream = tcpClient.GetStream();
            


            //first byte is command size; sending it over so that the server knows how to read the stream
            tcpStream.WriteByte((byte)timerInterval);

            //setting up the timer 
            //----------------------
            Timer tim = new Timer((double)timerInterval);
            tim.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            tim.Start();


            //----------------------
            Console.WriteLine("Press enter to stop the sending and see some stats\n");
            Console.ReadLine();
            tim.Stop();
            Console.WriteLine("Packets sent:{0}\nTimer interval (ms):{1}\nCommand size (bytes):{2}\n", packet_sent_count, timerInterval, commandSize);


            Console.WriteLine("Press enter again to exit");
            Console.ReadLine();

            
        }

        static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //send the command
            tcpStream.Write(command, 0, command.Length);
            //keep counting how many packets are sent
            packet_sent_count++;
        }
    }
}
