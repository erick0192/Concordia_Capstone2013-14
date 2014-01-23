using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using Rover.Commands;
using MarsRover;

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

            Thread dispatcher = new Thread(() => Dispatcher(CommanderDispatcherMessageQueue));
            Thread serialManager = new Thread(() => SerialManager(DispatcherSerialMessageQueue));

            dispatcher.Start();
            serialManager.Start();

            int TimeBetweenCommands = 100;

            
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


                //SendCommands to Dispatcher
                CommanderDispatcherMessageQueue.Enqueue(movementCommand);   
                CommanderDispatcherMessageQueue.Enqueue(camera1Command);
                CommanderDispatcherMessageQueue.Enqueue(camera2Command);
                CommanderDispatcherMessageQueue.Enqueue(camera3Command);
                CommanderDispatcherMessageQueue.Enqueue(camera4Command);
                CommanderDispatcherMessageQueue.Enqueue(camera5Command);
                
                Thread.Sleep(TimeBetweenCommands);
            }
  
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

                    Thread.Sleep(sleepPeriod); //Sleep for a bit and wait for data to fill up
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

    }

}
