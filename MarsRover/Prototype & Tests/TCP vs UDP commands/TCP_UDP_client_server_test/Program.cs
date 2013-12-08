using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;
using System.Text.RegularExpressions;

namespace TCP_UDP_client_server_test
{
    static class Client
    {
        public static void TCPconnect()
        {

        }

        public static void UDPconnect()
        {

        }

    }

    static class Server
    {
        public static void UDPlisten()
        {

        }

        public static void TCPlisten()
        {

        }

    }


    static class Options
    {
        public enum Mode { server, client };
        public enum Protocol { tcp, udp };

        static public Options.Mode mode;
        static public Options.Protocol protocol;
        static public int port = 5000;
        static public string ip = "10.10.10.10";
        static public int frequency = 100;
        static public int heartbeat_frequency = 250;
        static public int length = 150;
    }

    class Program
    {
        static void GetServerOrClient()
        {
        //THAT'S RIGHT, GOTOs EVERYWHERE; WHAT'RE YOU GOING TO DO ABOUT IT

            //server or client selection
        SERVER_OR_CLIENT:
            Console.Write(@"Server or client? ('s' or 'c'):");
            string modeInput = Console.ReadLine().Trim().ToLower();
            switch (modeInput)
            {
                case "s":
                    Options.mode = Options.Mode.server;
                    break;
                case "server":
                    Options.mode = Options.Mode.server;
                    break;
                case "c":
                    Options.mode = Options.Mode.client;
                    break;
                case "client":
                    Options.mode = Options.Mode.client;
                    break;
                default:
                    goto SERVER_OR_CLIENT;
            }
        }

        static void GetTCPorUDP()
        {
        //server or client selection
        TCP_OR_UDP:
            Console.Write(@"TCP or UDP? ('u' or 't'):");
            string proto_input = Console.ReadLine().Trim().ToLower();
            switch (proto_input)
            {
                case "t":
                    Options.protocol = Options.Protocol.tcp;
                    break;
                case "tcp":
                    Options.protocol = Options.Protocol.tcp;
                    break;
                case "u":
                    Options.protocol = Options.Protocol.udp;
                    break;
                case "udp":
                    Options.protocol = Options.Protocol.udp;
                    break;
                default:
                    goto TCP_OR_UDP;
            }
        }

        static void GetIP()
        {
            //get IP to connect to, if in client mode
            if (Options.mode == Options.Mode.client)
            {
            IP:
                Console.Write("IP plz[{0}]: ", Options.ip);
                string ipInput = Console.ReadLine().Trim();

                if (ipInput.Length > 0)
                {
                    Match match = Regex.Match(ipInput, @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
                    if (!(match.Success))
                    {
                        goto IP;
                    }
                    else
                    {
                        Options.ip = ipInput;
                    }
                }
            }
        }

        static void GetPort()
        {
         PORT:
            Console.Write("Port to listen on, or connect to[{0}]: ", Options.port);
            string portInput = Console.ReadLine().Trim();
            if (portInput.Length > 0)
            {
                int dumb;
                if(int.TryParse(portInput, out dumb))
                {
                    //65535 is the highest port available on TCP and UDP
                    if (dumb <= (65535))
                    {
                        Options.port = dumb;
                    }
                    else
                    {
                        goto PORT;
                    }
                }
                else
                {
                    goto PORT;
                }

            }
        }

        static void GetLength()
        {
        LENGTH:
            Console.Write("Please specify the length, in bytes, of the UDP frame or the size of TCP packet[{0}] (up to 255 bytes): ", Options.length);
            string lenInput = Console.ReadLine().Trim();
            int dumb;
            if (int.TryParse(lenInput, out dumb))
            {
                if (dumb <= 256)
                {
                    Options.length = dumb;
                }
                else
                {
                    goto LENGTH;
                }


            }
            else
            {
                goto LENGTH;
            }
        }

        static void GetFrequency()
        {
            FREQ:
            Console.Write("Please specify the frequency of sending frames or packets, in milliseconds[{0}]: ", Options.frequency);
            string freqInput = Console.ReadLine().Trim();
            int dumb;
            if (int.TryParse(freqInput, out dumb))
            {
                Options.frequency = dumb;
            }
            else
            {
                goto FREQ;
            }
        }

        static void GetHeartbeat()
        {
        FREQ_HEARTBEAT:
            Console.Write("Please specify the frequency of sending the heartbeat, in milliseconds[{0}]: ", Options.heartbeat_frequency);
            string freqInput = Console.ReadLine().Trim();
            int dumb;
            if (int.TryParse(freqInput, out dumb))
            {
                Options.heartbeat_frequency = dumb;
            }
            else
            {
                goto FREQ_HEARTBEAT;
            }
        }

        static void Main(string[] args)
        {
        //THAT'S RIGHT, GOTOs EVERYWHERE; WHAT'RE YOU GOING TO DO ABOUT IT
            MAIN:


            GetServerOrClient();         
            GetTCPorUDP();

            if (Options.mode == Options.Mode.server)
            {
                if (Options.protocol == Options.Protocol.tcp)
                {
                    //server, tcp
                    Server.TCPlisten();

                }
                else
                {
                    //sevver, udp
                    Server.UDPlisten();
                }
            }
            else
            {
                GetIP();
                GetPort();
                GetFrequency();
                GetLength();

                if (Options.protocol == Options.Protocol.tcp)
                {
                    //client, tcp
                    GetHeartbeat();
                    Client.TCPconnect();

                }
                else
                {
                    //client, udp
                    Client.UDPconnect();
                }
            }
            Console.Write(@"Enter the letter 'a' to restart, anything else to exit: ");
            string input = Console.ReadLine().Trim().ToLower();

            if (input.Length > 0 && input == "a")
            {
                goto MAIN;
            }
        }
    }
}
