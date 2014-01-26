using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using Rover.Commands;
using MarsRover;
using MarsRover.Commands;
using MarsRover.Communication;

namespace Rover
{
    //Proof of concept of how the final software will look like.
    //Most of this will probably be the final code, aside from the explicit use of ConcurrentQueues which will need to be hidden from the main code.
    //Also, the commander should probably remove the command headers instead of keep them there.
    class Program
    {
        
        static void Main(string[] args)
        {            
           // ConcurrentQueue<string> CommanderDispatcherMessageQueue = new ConcurrentQueue<string>();
           // ConcurrentQueue<string> DispatcherSerialMessageQueue = new ConcurrentQueue<string>();
            IQueue CommanderDispatcherMessageQueue = new PriorityQueue(30);
            IQueue DispatcherSerialMessageQueue = new PriorityQueue(30);
            IQueue SerialStatusMessageQueue = new PriorityQueue(30);

            Thread dispatcher = new Thread(() => Dispatcher(CommanderDispatcherMessageQueue));
            Thread serialManager = new Thread(() => SerialManager(DispatcherSerialMessageQueue));
            Thread statusUpdater = new Thread(() => StatusUpdater(SerialStatusMessageQueue));

            dispatcher.Start();
            //serialManager.Start();
            //statusUpdater.Start();

           
            //Start the commands listener
            var commandsListener = new MessageListener(
                Properties.Settings.Default.CommandsPort,
                CommanderDispatcherMessageQueue,
                Properties.Settings.Default.OperatorIPAddress);
            commandsListener.StartListening();
        }

        static void Dispatcher(IQueue MessageBox)
        {
            int sleepPeriod = 100;
            CommandFactory factory = new CommandFactory();
            string unparsedCommand = "";

            while (true)
            {
                if (MessageBox.TryDequeue(out unparsedCommand))
                {
                    ICommand command = factory.CreateCommand(unparsedCommand);
                    command.Execute();
                }
                else
                {

                    
                }
                
            }
        }

        static void SerialManager(IQueue DispatcherMessageBox)
        {
            MicrocontrollerSingleton microcontroller = MicrocontrollerSingleton.Instance;
            bool success = false;

            while (microcontroller.IsInitialized == false)
            {
                microcontroller.Initialize();
                if (microcontroller.IsInitialized == true)
                {
                    Console.WriteLine("Connected to microcontroller");
                    success = microcontroller.WriteMessage("test \n");
                    Console.WriteLine(success.ToString());
                }
                else
                {
                    Console.WriteLine("Could not connect to microcontroller");
                }
            }

            Thread.Sleep(5000);
        }

        static void StatusUpdater(IQueue SerialStatusUpdaterMessageBox)
        {
            Dictionary<string, ISensorLog> latestSensorData = new Dictionary<string, ISensorLog>();
            Initialize(latestSensorData); // initialize dictionary to set expected values

            //GPSLog gps = new GPSLog("G123,456,789");
            string banana;
            ISensorLog sensorlog;


            while (true)
            {
                while (SerialStatusUpdaterMessageBox.TryDequeue(out banana))
                {

                    if (banana[0] == 'G') //perhaps put all of this into a factory
                    {
                        sensorlog = new GPSLog(banana);
                    }
                    else
                    {
                        sensorlog = new GPSLog("G9,999,999");
                        //do nothing for now
                    }
                    latestSensorData[sensorlog.Identifier] = sensorlog;
                }
              //  Console.WriteLine(banana);
               // gps.UpdateValues();

                Thread.Sleep(50);
            }


            //read values from microcontroller
            //store in dictionary
            //display data
            //unupdate sensor readings
        }

        public static void Initialize(Dictionary<string, ISensorLog> dict)
        {
            GPSLog garbage = new GPSLog("G000,000,000");
            dict.Add("G", garbage);
        }
    }

}
