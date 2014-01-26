using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MarsRover;

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
            throw new NotImplementedException();
        }

        public void UnExecute()
        {
            throw new NotImplementedException("There is no unexecute for pan commands");
        }
    }

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
            catch (ArgumentException)
            {
                throw;
            }
            microcontroller = MicrocontrollerSingleton.Instance;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void UnExecute()
        {
            throw new NotImplementedException("There is no unexecute for tilt commands");
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

            int readLength = CommandMetadata.Tilt.AngleEndIndex - CommandMetadata.Tilt.AngleStartIndex +1;

            tiltAngleStr = unparsedText.Substring(CommandMetadata.Tilt.AngleStartIndex, readLength);

            try
            {
                tiltAngleNum = Convert.ToInt32(tiltAngleStr);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid Pan angle received in " + rawCommand);
            }

            if (tiltAngleNum > CommandMetadata.Tilt.MaxTiltAngle)
            {
                throw new ArgumentException("Pan angle received higher than expected maximum in " + rawCommand);
            }
            else if (tiltAngleNum < CommandMetadata.Tilt.MinTiltAngle)
            {
                throw new ArgumentException("Pan angle received lower than expected minimum in" + rawCommand);
            }

            return tiltAngleNum;
        }
    }

    public class CameraCommand : ICommand
    {
        private int camIndex;
        private bool status;
        public bool CameraStatus { get { return status; } }
        public int CameraIndex { get { return camIndex; } }
      
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
            catch(Exception)
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
        public string RawCommand {get { return rawCommand; } }
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
