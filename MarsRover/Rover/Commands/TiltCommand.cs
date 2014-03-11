using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover.Commands;

namespace Rover.Commands
{
    public class TiltCommand : ICommand
    {
        private int camIndex;
        private int tiltAngle;
        private string rawCommand;
        private MicrocontrollerSingleton microcontroller;

        public int CameraIndex { get { return camIndex; } }
        public int Angle { get { return tiltAngle; } }
        public string RawCommand { get { return rawCommand; } }

        public TiltCommand(string unparsedCommand)
        {
            if (unparsedCommand == null)
            {
                throw new ArgumentNullException("Null string received");
            }

            rawCommand = unparsedCommand;
            try
            {
                camIndex = ParseCamIndex(unparsedCommand);
                tiltAngle = ParseTiltAngle(unparsedCommand);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            microcontroller = MicrocontrollerSingleton.Instance;
        }

        public void Execute()
        {
            string message = CreateMessage();


            //Send message to serial port / serial handler
            if (microcontroller.IsInitialized)
            {
                microcontroller.WriteMessage(message);
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine(message); //Stub. Remove for final version.
            }
        }

        public void UnExecute()
        {
            throw new NotImplementedException("There is no unexecute for tilt commands");
        }

        private string CreateMessage()
        {
            return CommandMetadata.StartDelimiter + CommandMetadata.Tilt.Identifier + camIndex.ToString() + Angle.ToString() + CommandMetadata.EndDelimiter;
        }

        private int ParseCamIndex(string unparsedCommand)
        {
            string rawCamIndex;
            int index;

            rawCamIndex = unparsedCommand.Substring(CommandMetadata.Tilt.NumberIdentifierIndex, CommandMetadata.Tilt.NumberIdentifierLength);

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

        private int ParseTiltAngle(string unparsedText)
        {
            string tiltAngleStr;
            int tiltAngleNum;

            int readLength = CommandMetadata.Tilt.AngleEndIndex - CommandMetadata.Tilt.AngleStartIndex + 1;

            tiltAngleStr = unparsedText.Substring(CommandMetadata.Tilt.AngleStartIndex, readLength);

            try
            {
                tiltAngleNum = Convert.ToInt32(tiltAngleStr);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid tilt angle received in " + rawCommand);
            }

            if (tiltAngleNum > CommandMetadata.Tilt.MaxTiltAngle)
            {
                throw new ArgumentException("Tilt angle received higher than expected maximum in " + rawCommand);
            }
            else if (tiltAngleNum < CommandMetadata.Tilt.MinTiltAngle)
            {
                throw new ArgumentException("Tilt angle received lower than expected minimum in" + rawCommand);
            }

            return tiltAngleNum;
        }
    }
}
