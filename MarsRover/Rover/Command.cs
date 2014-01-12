using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rover
{
    public interface ICommand
    {

        //Expected commands to be implemented:
        //Movement commands
        //Arm commands
        //Camera commands
        //Servo commands
        //End effector commands
        //Collection bin commands

        void Execute();
        void UnExecute();
    }

    public class NullCommand : ICommand
    {
        public NullCommand(string unparsedText)
        {
            return; //do nothing
        }

        public NullCommand()
        {
            return;
        }

        public void Execute()
        {
            return;
        }

        public void UnExecute()
        {
            return;
        }
    }

    public class CameraCommand : ICommand
    {
        private int camIndex;
        private bool status;

        public CameraCommand(string unparsedCommand)
        {
            try
            {
                camIndex = getCamIndex(unparsedCommand);
                status = getCamStatus(unparsedCommand);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw e;
            }
        }

        public void Execute()
        {
            Console.WriteLine("Turning Camera {0} {1}", this.camIndex, this.status == true ? "On" : "Off");
        }

        public void UnExecute()
        {
            throw new NotImplementedException("There is no unexecute for camera commands");
        }

        private int getCamIndex(string unparsedCommand)
        {
            string rawCamIndex = unparsedCommand.Substring(CommandMetadata.Camera.NumberIndex, CommandMetadata.Camera.NumberLength);
            int index;
            try
            {
                index = Convert.ToInt32(rawCamIndex);
            }
            catch(Exception)
            {
                throw new ArgumentOutOfRangeException("Invalid camera index " + rawCamIndex + " received for command " + unparsedCommand);
            }

            return index;
        }

        private bool getCamStatus(string unparsedCommand)
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

        public int getCamIndex()
        {
            return this.camIndex;
        }

        public bool getCamStatus()
        {
            return this.status;
        }
    }

    public class MovementCommand : ICommand
    {
        //Todo: add error checking on string-based instantiation
        //It is assumed that new commands will be created each time rather than the caller re-using a previously existing one.

        private char rightDirection;
        private char leftDirection;
        private int rightSpeed;
        private int leftSpeed;

        public MovementCommand(char leftDirection, int leftValue, char rightDirection, int rightValue)
        {
            if (leftDirection == 'F' || leftDirection == 'B')
            {
                this.leftDirection = leftDirection;
            }
            else
            {
                throw new ArgumentOutOfRangeException(leftDirection.ToString(), leftDirection, leftDirection.ToString() + " is not a valid direction");
            }

            if (leftValue >= 0 && leftValue <= 255)
            {
                this.leftSpeed = leftValue;
            }
            else
            {
                throw new ArgumentOutOfRangeException(leftValue.ToString(), leftValue, leftValue.ToString() + "Received invalid left speed value: " + leftValue.ToString());
            }

            if (rightDirection == 'F' || rightDirection == 'B')
            {
                this.rightDirection = rightDirection;
            }
            else
            {
                throw new ArgumentOutOfRangeException(rightDirection.ToString(), rightDirection, rightDirection.ToString() + " is not a valid direction");
            }

            if (rightValue >= 0 && rightValue <= 255)
            {
                this.rightSpeed = rightValue;
            }
            else
            {
                throw new ArgumentOutOfRangeException(rightValue.ToString(), rightValue, "Received invalid right speed value: " + rightValue.ToString());
            }


        }

        public MovementCommand(string unparsedText)
        {
            rightDirection = ParseRightDirection(unparsedText);
            rightSpeed = ParseRightSpeed(unparsedText);
            leftDirection = ParseLeftDirection(unparsedText);
            leftSpeed = ParseLeftSpeed(unparsedText);
        }


         public void Execute()
        {
            //Construct message to send to microcontroller
            string message = CreateMessage();


            //Send message to serial port / serial handler
            SendMessage(message); //stub
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

        public char GetLeftDirection()
        {
            return this.leftDirection;
        }

        public char GetRightDirection()
        {
            return this.rightDirection;
        }

        public int GetLeftSpeed()
        {
            return this.leftSpeed;
        }

        public int GetRightSpeed()
        {
            return this.rightSpeed;
        }


        private string CreateMessage()
        {
            //Messages should be of the format "<LeftDirection LeftValue RightDirection RightValue>" (no spaces)
            //Ex: <F255F255> for full speed ahead

            return "<M" + leftDirection.ToString() + leftSpeed.ToString("D3") + rightDirection.ToString() + rightSpeed.ToString("D3") + ">";
        }

        private void SendMessage(string message)
        {
            Console.WriteLine(message);
        }

        private char ReverseDirection(char direction)
        {
            if (direction == 'F')
                return 'B';
            else
                return 'F';
        }

        private char ParseRightDirection(string text)
        {
            return text[CommandMetadata.Movement.RightDirectionIndex];
        }

        private char ParseLeftDirection(string text)
        {
            return text[CommandMetadata.Movement.LeftDirectionIndex];
        }

        private int ParseRightSpeed(string text)
        {
            string rightSpeedStr = "";
            int rightSpeedNum;

            for (int i = CommandMetadata.Movement.RightSpeedStartIndex; i <= CommandMetadata.Movement.RightSpeedEndIndex; i++)
            {
                rightSpeedStr += text[i];
            }

            rightSpeedNum = Convert.ToInt32(rightSpeedStr);

            return rightSpeedNum;

        }

        private int ParseLeftSpeed(string text)
        {
            string leftSpeedStr = "";
            int leftSpeedNum;

            for (int i = CommandMetadata.Movement.LeftSpeedStartIndex; i <= CommandMetadata.Movement.LeftSpeedEndIndex; i++)
            {
                leftSpeedStr += text[i];
            }

            leftSpeedNum = Convert.ToInt32(leftSpeedStr);

            return leftSpeedNum;
        }

    }
}
