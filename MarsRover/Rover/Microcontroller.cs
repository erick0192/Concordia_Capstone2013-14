using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Rover
{
    class MicrocontrollerSingleton
    {
        private SerialPort serialPort;
        private MicrocontrollerSingleton instance;
        private bool ready = false;


        public bool Initialize()
        {
            int retry = 10000;
            bool success = false;

            while (retry > 0 && success == false)
            {
                if (!ready)
                {
                    string[] portnames = SerialPort.GetPortNames();
                    if (portnames.Length > 0)
                    {
                        foreach (string port in portnames)
                        {
                            serialPort.Open();
                        }
                    }

                }

                retry -= 1;
            }

            return success;
        }

        public void WriteMessage()
        {

        }

        public void ReadMessage()
        {

        }

        public void CloseCommunications()
        {
        }
    }

}

