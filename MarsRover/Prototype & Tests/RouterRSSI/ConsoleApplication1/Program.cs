using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Communication;

namespace MinimalisticTelnet
{
    class Program
    {       
        static void Main(string[] args)
        {
            //RouterCnx Router = new RouterCnx("10.10.10.1","admin","iloverobotics");
            RouterCnx Router = new RouterCnx("127.0.0.1", "admin", "iloverobotics");

            if (Router.Connect() == true)
            {
                Console.WriteLine("Connection established");
            }
            else
            {
                Console.WriteLine("Connection failed");
            }
            
            while(true)
            {
                //Insert the Rover MAC Adress here.
                int RSSIString = Router.GetRSSIValue("EC:55:F9:6D:A3:F8");
                
                Console.WriteLine(RSSIString.ToString() + "dbm");
                
                Thread.Sleep(500);
            }
        }

    }
}
