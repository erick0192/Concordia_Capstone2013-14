using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover;
using MarsRover.Commands;

namespace Rover.Commands
{
    public class CameraCommand : ICommand
    {
        private int camIndex;
        private bool status;
        public bool CameraStatus { get { return status; } }
        public int CameraIndex { get { return camIndex; } }

        private MicrocontrollerSingleton microcontroller;

        public CameraCommand(string unparsedCommand)
        {
            try
            {
                camIndex = ParseCameraIndex(unparsedCommand);
                status = ParseCameraStatus(unparsedCommand);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        public void Execute()
        {
            Console.WriteLine("Turning Camera {0} {1}", this.camIndex, this.status == true ? "On" : "Off");

            if (this.camIndex <= RoverCameraFactory.GetInstance().GetCameras().Count - 1)
            {
                RoverCameraDevice c = RoverCameraFactory.GetInstance().GetCameras().ElementAt(this.camIndex);

                if (this.status == true)
                {
                    c.Start(c.GetCapabilities(new Size(320, 240)));
                }
                else
                {
                    c.Stop();
                }
            }

        }

        public void UnExecute()
        {
            throw new NotImplementedException("There is no unexecute for camera commands");
        }

        private int ParseCameraIndex(string unparsedCommand)
        {
            string rawCamIndex = unparsedCommand.Substring(CommandMetadata.Camera.NumberIndex, CommandMetadata.Camera.NumberLength);
            int index;
            try
            {
                index = Convert.ToInt32(rawCamIndex);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid camera index " + rawCamIndex + " received for command " + unparsedCommand);
            }

            return index;
        }

        private bool ParseCameraStatus(string unparsedCommand)
        {
            string rawStatus = unparsedCommand.Substring(CommandMetadata.Camera.StatusIndex, CommandMetadata.Camera.StatusLength);

            if (rawStatus == CommandMetadata.Camera.On)
            {
                return true;
            }
            else if (rawStatus == CommandMetadata.Camera.Off)
            {
                return false;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid camera status " + rawStatus + " received for command " + unparsedCommand);
            }
        }

    }
}
