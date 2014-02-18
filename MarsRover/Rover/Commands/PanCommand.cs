using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover.Commands;

namespace Rover.Commands
{
    public class PanCommand : ICommand
    {
        private int camIndex;
        private int panAngle;

        private string rawCommand;
        private MicrocontrollerSingleton microcontroller;

        public int CameraIndex { get { return camIndex; } }
        public int Angle { get { return panAngle; } }
        public string RawCommand { get { return rawCommand; } }

        public PanCommand(string unparsedCommand)
        {
            //Extra error handling might be needed on a per-servo basis (ex: some servos are expected to do 360 degrees while others arent)
            if (unparsedCommand == null)
            {
                throw new ArgumentNullException("Null string received");
            }

            rawCommand = unparsedCommand;
            try
            {
                camIndex = ParseCamIndex(unparsedCommand);
                panAngle = ParsePanAngle(unparsedCommand);
            }
            catch (ArgumentException)
            {
                throw;
            }
            microcontroller = MicrocontrollerSingleton.Instance;
        }

        private int ParseCamIndex(string unparsedCommand)
        {
            string rawCamIndex;
            int index;

            rawCamIndex = unparsedCommand.Substring(CommandMetadata.Pan.NumberIdentifierIndex, CommandMetadata.Pan.NumberIdentifierLength);

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

        private string CreateMessage()
        {
            return CommandMetadata.StartDelimiter + CommandMetadata.Pan.Identifier + camIndex.ToString() + Angle.ToString() + CommandMetadata.EndDelimiter;
        }

        private int ParsePanAngle(string unparsedText)
        {
            string panAngleStr;
            int panAngleNum;

            int readLength = CommandMetadata.Pan.AngleEndIndex - CommandMetadata.Pan.AngleStartIndex + 1;

            panAngleStr = unparsedText.Substring(CommandMetadata.Pan.AngleStartIndex, readLength);

            try
            {
                panAngleNum = Convert.ToInt32(panAngleStr);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid Pan angle received in " + rawCommand);
            }

            if (panAngleNum > CommandMetadata.Pan.MaxPanAngle)
            {
                throw new ArgumentException("Pan angle received higher than expected maximum in " + rawCommand);
            }
            else if (panAngleNum < CommandMetadata.Pan.MinPanAngle)
            {
                throw new ArgumentException("Pan angle received lower than expected minimum in" + rawCommand);
            }

            return panAngleNum;
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
            throw new NotImplementedException("There is no unexecute for pan commands");
        }
    }
}
