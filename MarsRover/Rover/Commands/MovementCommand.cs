using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover.Commands
{
    public class MovementCommand : ICommand
    {
        //Todo: add error checking on string-based instantiation
        //It is assumed that new commands will be created each time rather than the caller re-using a previously existing one.

        private string rightDirection;
        private string leftDirection;
        private int rightSpeed;
        private int leftSpeed;
        private string rawCommand;

        public string RightDirection { get { return rightDirection; } }
        public string LeftDirection { get { return leftDirection; } }
        public int RightSpeed { get { return rightSpeed; } }
        public int LeftSpeed { get { return leftSpeed; } }
        public string RawCommand { get { return rawCommand; } }
        private MicrocontrollerSingleton microcontroller;

        public MovementCommand(string unparsedText)
        {
            if (unparsedText == null)
            {
                throw new ArgumentNullException("Null string received");
            }
            if (IsValidIdentifier(unparsedText) == false)
            {
                throw new ArgumentException("Invalid command identifier");
            }

            rawCommand = unparsedText;
            try
            {
                rightDirection = ParseRightDirection(rawCommand);
                rightSpeed = ParseRightSpeed(rawCommand);
                leftDirection = ParseLeftDirection(rawCommand);
                leftSpeed = ParseLeftSpeed(rawCommand);
            }
            catch (ArgumentException)
            {
                throw;
            }
            microcontroller = MicrocontrollerSingleton.Instance;

        }

        public void Execute()
        {
            //Construct message to send to microcontroller
            string message = CreateMessage();


            //Send message to serial port / serial handler
            if (microcontroller.IsInitialized)
            {
                microcontroller.WriteMessage(message);
            }
            else
            {
                SendMessage(message); //Stub. Remove for final version.
            }
        }

        public void UnExecute()
        {
            //Unexecution implies sending the reverse direction of the velocity vector (ie. go backwards at the same speed as before).
            //The duration of how long this unexecution should last is determined by the same entity that decided how long the original
            //command should last for.
            this.leftDirection = ReverseDirection(this.leftDirection);
            this.rightDirection = ReverseDirection(this.rightDirection);

            this.Execute();
        }


        private string CreateMessage()
        {
            //Messages should be of the format "<LeftDirection LeftValue RightDirection RightValue>" (no spaces)
            //Ex: <F255F255> for full speed ahead

            return CommandMetadata.StartDelimiter + CommandMetadata.Movement.Identifier + leftDirection.ToString() + leftSpeed.ToString("D3") + rightDirection.ToString() + rightSpeed.ToString("D3") + CommandMetadata.EndDelimiter;
        }

        private void SendMessage(string message)
        {
            Console.WriteLine(message);
        }

        private string ReverseDirection(string direction)
        {
            if (direction == CommandMetadata.Movement.Forward)
                return CommandMetadata.Movement.Backward;
            else
                return CommandMetadata.Movement.Forward;
        }

        private bool IsValidIdentifier(string text)
        {
            string identifier = text.Substring(CommandMetadata.IdIndex, CommandMetadata.IdLength);
            if (identifier == CommandMetadata.Movement.Identifier)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private string ParseRightDirection(string text)
        {
            string direction = text.Substring(CommandMetadata.Movement.RightDirectionIndex, CommandMetadata.Movement.DirectionLength);
            if (direction == CommandMetadata.Movement.Forward || direction == CommandMetadata.Movement.Backward)
            {
                return direction;
            }
            else
            {
                throw new ArgumentException("Invalid Right Movement Direction Received");
            }
        }

        private string ParseLeftDirection(string text)
        {
            string direction = text.Substring(CommandMetadata.Movement.LeftDirectionIndex, CommandMetadata.Movement.DirectionLength);
            if (direction == CommandMetadata.Movement.Forward || direction == CommandMetadata.Movement.Backward)
            {
                return direction;
            }
            else
            {
                throw new ArgumentException("Invalid Left Movement Direction Received");
            }
        }

        private int ParseRightSpeed(string text)
        {
            string rightSpeedStr = "";
            int rightSpeedNum;

            rightSpeedStr = text.Substring(CommandMetadata.Movement.RightSpeedStartIndex, CommandMetadata.Movement.MaxSpeed.ToString().Length);
            rightSpeedNum = Convert.ToInt32(rightSpeedStr);

            if (rightSpeedNum > CommandMetadata.Movement.MaxSpeed)
            {
                throw new ArgumentException("Speed received higher than expected maximum");
            }

            return rightSpeedNum;

        }

        private int ParseLeftSpeed(string text)
        {
            string leftSpeedStr = "";
            int leftSpeedNum;

            leftSpeedStr = text.Substring(CommandMetadata.Movement.LeftSpeedStartIndex, CommandMetadata.Movement.MaxSpeed.ToString().Length);
            leftSpeedNum = Convert.ToInt32(leftSpeedStr);

            if (leftSpeedNum > CommandMetadata.Movement.MaxSpeed)
            {
                throw new ArgumentException("Speed received higher than expected maximum");
            }

            return leftSpeedNum;
        }
    }
}
