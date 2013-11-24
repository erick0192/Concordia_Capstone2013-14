using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace UDP_Send
{
    class Program
    {
        static byte[] command;
        static void Main(string[] args)
        {
            double timerInterval = 1000;
            

            //set the command size in terms of bytes
            int commandSize = 20;
            command = new byte[commandSize];

            //filling up the command with the value of the timer interval instead of an actual command;
            //good way to tell the receiving end what our interval is on this side
            Array.Copy(BitConverter.GetBytes(commandSize), command, BitConverter.GetBytes(commandSize).Length);

            //setting up the timer
            Timer tim = new Timer(timerInterval);
            tim.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            tim.Start();
            
            
            
            while(true);
            
        }

        static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("time up");
            
        }
    }
}
