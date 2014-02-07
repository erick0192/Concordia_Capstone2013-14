using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Rover.Commands;
using MarsRover.Commands;
using MarsRover;
namespace Rover
{
    public class MicrocontrollerSingleton
    {
        private SerialPort serialPort;
        private static volatile MicrocontrollerSingleton instance; //Using the example mentioned on the Microsoft C# guide for Singleton Implementation
        private bool isInitialized;
        private static object syncLock = new Object();
        private static object writeLock = new Object();
        private string commandRead;
        public bool IsInitialized { get { return isInitialized; } }
        private IQueue MessageQueue = null;

        public static MicrocontrollerSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncLock)//Only go through the trouble of setting the lock if we read that the instance is null
                    {
                        if (instance == null) //Check to make sure that it's still null
                        {
                            instance = new MicrocontrollerSingleton();
                        }
                    }
                }

                return instance;
            }

        }

        private MicrocontrollerSingleton()
        {
            serialPort = new SerialPort();
            isInitialized = false;
            commandRead = "";
        }

        public bool Initialize()
        {

            if (isInitialized)
            {
                return true;
            }
            else
            {
                lock (syncLock)
                {
                    if (isInitialized == false)
                    {
                        int retry = 10000;

                        while (retry > 0 && isInitialized == false)
                        {
                            string[] portnames = SerialPort.GetPortNames();
                            if (portnames.Length > 0)
                            {
                                foreach (string port in portnames)
                                {
                                    try
                                    {
                                        //Assumes only 1 device is connected via serial. Some additional feedback will have to be sent from
                                        //the arduino if more than one serial device is connected.

                                        serialPort.PortName = port;
                                        serialPort.BaudRate = 115200;
                                        serialPort.DataReceived += new SerialDataReceivedEventHandler(CommandReadyEvent);
                                        serialPort.Open();
                                        serialPort.DiscardOutBuffer();
                                        isInitialized = true;
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message); //log error here
                                    }
                                }
                            }
                            retry -= 1;
                        }
                    }
                }

            }
  
            return isInitialized;
        }

        public void SetQueue(IQueue Messagebox)
        {
            this.MessageQueue = Messagebox;
        }

        public bool Initialize(string portName, int baud = 9600, int readTimeout = 500, int writeTimeout = 500)
        {

            if (isInitialized)
            {
                return true;
            }
            else
            {
                lock (syncLock)
                {
                    if (isInitialized == false)
                    {
                        int retry = 10000;

                        while (retry > 0 && isInitialized == false)
                        {
                            try
                            {
                                serialPort.PortName = portName;
                                serialPort.BaudRate = baud;
                                serialPort.ReadTimeout = readTimeout;
                                serialPort.WriteTimeout = writeTimeout;
                                serialPort.DataReceived += new SerialDataReceivedEventHandler(CommandReadyEvent);
                                serialPort.Open();
                                serialPort.DiscardOutBuffer();

                                isInitialized = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message); //log error here
                            }

                            retry -= 1;
                        }
                    }
                }
            }

            return isInitialized;
        }

        public bool WriteMessage(string message)
        {
            if (isInitialized == true && serialPort.IsOpen == true)
            {
                lock (writeLock)
                {
                    try
                    {
                        serialPort.Write(message);
                    }
                    catch (InvalidOperationException e)
                    {
                        isInitialized = false;
                        Console.WriteLine("{0}: " + e.Message, DateTime.Now); //Log error here
                        throw;
                    }
                    catch (TimeoutException e)
                    {
                        Console.WriteLine("{0}: " + e.Message, DateTime.Now); //Log error here
                        throw;
                    }
                }

                return true;
            }
            else
            {
                isInitialized = false;
                return false;
            }
        }

        private void CommandReadyEvent(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            commandRead = sp.ReadTo("|"); //set to const later
            commandRead = commandRead.Replace("\n", "");
            commandRead = commandRead.Replace("\r", "");
            //commandRead += CommandMetadata.EndDelimiter;
            commandRead = "<" + commandRead + ">";
            if (MessageQueue != null)
            {
                MessageQueue.Enqueue(commandRead);
                Console.WriteLine(commandRead);
            //    Console.WriteLine("banana");
            }
            else
            {
                Console.WriteLine(commandRead); //Replace with enqueueing data
            }
        }



        public void CloseCommunications()
        {
            serialPort.Close();
            isInitialized = false;
        }
    }

}

