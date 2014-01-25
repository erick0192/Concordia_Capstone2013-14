using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace UDPMessageSender
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 0;
            string ipAddrStr;
            string portStr = "";
            int timeBetweenCommands = 0;
            string timeStr = "";
            IPAddress ipAddr;

            Console.WriteLine("IP: ");
            ipAddrStr = Console.ReadLine();

           if (ipAddrStr == "localhost")
            {
                ipAddrStr = "127.0.0.1";
            }

            while (IPAddress.TryParse(ipAddrStr, out ipAddr) == false)
            {
                Console.WriteLine("Invalid IP Address. Please Input a new one: ");
                ipAddrStr = Console.ReadLine();
                if (ipAddrStr == "localhost")
                {
                    ipAddrStr = "127.0.0.1";
                }
            }

            Console.Write("Port: ");
            portStr = Console.ReadLine();

            while(int.TryParse(portStr, out port) == false || IsBetween(port, 1024, 65535) == false)
            {
                Console.Write("Invalid port. Please input a new one: ");
                portStr = Console.ReadLine();
                Console.WriteLine();
            }

            Console.WriteLine("Time Between Commands: ");
            timeStr = Console.ReadLine();

            while (int.TryParse(timeStr, out timeBetweenCommands) == false || IsBetween(timeBetweenCommands, 1, 20000) == false)
            {
                Console.WriteLine("Invaid time. Please input a new one: ");
                timeStr = Console.ReadLine();
            }

            UdpClient udpClient = new UdpClient();

            try
            {
                udpClient.Connect(ipAddr, port);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


            //Dummy for the commander. Consider building a full test program which communicates fake data over UDP to better simulate
            //The operating environment. This will also allow us to test out the watchdog.


            //Simulate User Input
            int movementLeft = 0, movementRight = 0;
            bool cam1Status = false,
                 cam2Status = false,
                 cam3Status = false,
                 cam4Status = false,
                 cam5Status = false;

            string movementCommand, camera1Command, camera2Command, camera3Command, camera4Command, camera5Command;
            var r = new Random();


            while (true)
            {

                //Randomize data to make it seem as if user is actually doing stuff
                movementLeft += r.Next(-10, 11);

                if (movementLeft > 255)
                    movementLeft = 255;
                else if (movementLeft < 0)
                    movementLeft = 0;

                movementRight += r.Next(-10, 11);
                if (movementRight > 255)
                    movementRight = 255;
                else if (movementRight < 0)
                    movementRight = 0;

                if (r.Next(0, 101) == 5) //Randomize camera actions
                    cam1Status = (r.Next(0, 2) == 0 ? false : true);

                if (r.Next(0, 101) == 5) //Randomize camera actions
                    cam2Status = (r.Next(0, 2) == 0 ? false : true);

                if (r.Next(0, 101) == 5)
                    cam3Status = (r.Next(0, 2) == 0 ? false : true);

                if (r.Next(0, 101) == 5)
                    cam4Status = (r.Next(0, 2) == 0 ? false : true);

                if (r.Next(0, 101) == 5)
                    cam5Status = (r.Next(0, 2) == 0 ? false : true);

                //Smooth data to make it seem like theyre moving the controller as opposed to putting in random values

                //Build commands
                movementCommand = "<MF" + movementLeft.ToString("D3") + "F" + movementRight.ToString("D3") + ">";
                camera1Command = "<C1" + (cam1Status == false ? "F" : "O") + ">";
                camera2Command = "<C2" + (cam2Status == false ? "F" : "O") + ">";
                camera3Command = "<C3" + (cam3Status == false ? "F" : "O") + ">";
                camera4Command = "<C4" + (cam4Status == false ? "F" : "O") + ">";
                camera5Command = "<C5" + (cam5Status == false ? "F" : "O") + ">";

                string[] Messages;
                Messages = new string[6];
                Messages[0] = movementCommand;
                Messages[1] = camera1Command;
                Messages[2] = camera2Command;
                Messages[3] = camera3Command;
                Messages[4] = camera4Command;
                Messages[5] = camera5Command;

                foreach (string message in Messages)
                {
                    Byte[] msgs = Encoding.ASCII.GetBytes(message);
                    udpClient.Send(msgs, msgs.Length);
                }

                Thread.Sleep(timeBetweenCommands);
            }
        }

        private static bool IsBetween(int port, int p1, int p2)
        {
            return (port >= p1 && port <= p2);
        }
    }
}
