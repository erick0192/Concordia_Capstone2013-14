using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover.Commands;

namespace Rover.Commands
{
    public class MovementCommand : ICommand
    {
        //It is assumed that new commands will be created each time rather than the caller re-using a previously existing one.

        private string motor1Direction;
        private string motor2Direction;
        private string motor3Direction;

        private string motorSide;

        private int motor1Speed;
        private int motor2Speed;
        private int motor3Speed;

        private string rawCommand;

        public string Motor1Direction { get { return motor1Direction; } }
        public string Motor2Direction { get { return motor2Direction; } }
        public string Motor3Direction { get { return motor3Direction; } }

        public string MotorSide { get { return motorSide; } }

        public int Motor1Speed { get { return motor1Speed; } }
        public int Motor2Speed { get { return motor2Speed; } }
        public int Motor3Speed { get { return motor3Speed; } }

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
                throw new ArgumentException("Invalid identifier found in command: " + unparsedText);
            }

            rawCommand = unparsedText;
            try
            {
                motorSide = ParseMotorSide(rawCommand);

                motor1Direction = ParseDirection(rawCommand, 0);
                motor2Direction = ParseDirection(rawCommand, 1);
                motor3Direction = ParseDirection(rawCommand, 2);

                foreach (string motorDirectionReading in new string[] {motor1Direction, motor2Direction, motor3Direction})
                {
                    if(motorDirectionReading != CommandMetadata.Movement.Forward && motorDirectionReading != CommandMetadata.Movement.Backward)
                    {
                        throw new ArgumentException("Invalid direction value found in command: " + unparsedText);
                    }
                }
                
                motor1Speed = ParseSpeed(rawCommand, 0);
                motor2Speed = ParseSpeed(rawCommand, 1);
                motor3Speed = ParseSpeed(rawCommand, 2);

                foreach (int motorSpeedReading in new int[]{motor1Speed, motor2Speed, motor3Speed})
                {
                    if (motorSpeedReading > CommandMetadata.Movement.MaxSpeed || motorSpeedReading < CommandMetadata.Movement.MinSpeed)
                    {
                        throw new ArgumentException("Invalid speed value found in command: " + unparsedText);
                    }
                }

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
            motor1Direction = ReverseDirection(motor1Direction);
            motor2Direction = ReverseDirection(motor2Direction);
            motor3Direction = ReverseDirection(motor3Direction);

            this.Execute();
        }

        private string CreateMessage()
        {
            //Ex: <LF255F255F255> for full speed ahead for the left side

            return CommandMetadata.StartDelimiter + motorSide + motor1Direction + motor1Speed.ToString("D3") + motor2Direction + motor2Speed.ToString("D3") +  motor3Direction + motor3Speed.ToString("D3") + CommandMetadata.EndDelimiter;
        }

        private string ParseMotorSide(string text)
        {
            return text[CommandMetadata.Movement.MotorSideIndex].ToString();
        }

        private void SendMessage(string message)
        {
            Console.WriteLine(message);
        }

        private string ReverseDirection(string direction)
        {
            if (direction == CommandMetadata.Movement.Forward)
            {
                return CommandMetadata.Movement.Backward;
            }
            else if (direction == CommandMetadata.Movement.Backward)
            {
                return CommandMetadata.Movement.Forward;
            }
            else
            {
                throw new ArgumentException("Invalid direction received");
            }
        }

        private bool IsValidIdentifier(string text)
        {
            string identifier = text.Substring(CommandMetadata.IdIndex, CommandMetadata.IdLength);
            if (identifier == CommandMetadata.Movement.RightIdentifier || identifier == CommandMetadata.Movement.LeftIdentifier)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private string ParseDirection(string text, int motorIndex)
        {
            if (motorIndex > 2 || motorIndex < 0)
            {
                throw new ArgumentException("Invalid Index Received in command: " + text);
            }
            string[] motorDirection = new string[3];

            motorDirection[0] = text[CommandMetadata.Movement.Motor1DirectionIndex].ToString();
            motorDirection[1] = text[CommandMetadata.Movement.Motor2DirectionIndex].ToString();
            motorDirection[2] = text[CommandMetadata.Movement.Motor3DirectionIndex].ToString();

            return motorDirection[motorIndex];
        }

        private int ParseSpeed(string text, int motorIndex)
        {
            if (motorIndex > 2 || motorIndex < 0)
            {
                throw new ArgumentException("Invalid index received in command: " + text);
            }

            int[] motorSpeed = new int[3];
            try
            {
                motorSpeed[0] = int.Parse(text.Substring(CommandMetadata.Movement.Motor1SpeedStartIndex, CommandMetadata.Movement.MaxSpeed.ToString().Length));
                motorSpeed[1] = int.Parse(text.Substring(CommandMetadata.Movement.Motor2SpeedStartIndex, CommandMetadata.Movement.MaxSpeed.ToString().Length));
                motorSpeed[2] = int.Parse(text.Substring(CommandMetadata.Movement.Motor3SpeedStartIndex, CommandMetadata.Movement.MaxSpeed.ToString().Length));
            }
            catch (FormatException)
            {
                throw new ArgumentException("Command improperly formatted in command: " + text);
            }

            return motorSpeed[motorIndex];
             
        }
    }
}
