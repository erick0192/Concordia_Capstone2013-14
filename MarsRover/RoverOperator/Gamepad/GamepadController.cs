using System;
using System.Threading;
using NLog;
using SharpDX;
using SharpDX.XInput;
using System.Collections.Generic;
using System.Text;
using MarsRover;

namespace RoverOperator.Gamepad
{
    class GamepadController
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private const int POLLING_RATE = 200; //milliseconds
        private int selectedCamera;
        private UDPSender udpSender;

        public GamepadController()
        {
            //TODO: Detect new controllers after start ?
            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two) };
            StartPollingAndSendingCommands(controllers);
        }

        public void StartPollingAndSendingCommands(Controller[] controllers)
        {
            if (controllers[0].IsConnected)
            {
                logger.Debug("Found a XInput controller available");
                Thread t = new Thread(() => PollAndSendMovementCommands(controllers[0]));
                t.IsBackground = true;
                t.Start();
            }

            if (controllers[1].IsConnected)
            {
                logger.Debug("Found a XInput controller available");
                Thread t = new Thread(() => PollAndSendCameraCommands(controllers[1]));
                t.IsBackground = true;
                t.Start();
            }
        }

        private void PollAndSendMovementCommands(Controller controller)
        {
            var previousState = controller.GetState();
            while (controller.IsConnected)
            {
                var state = controller.GetState();
                double LX = state.Gamepad.LeftThumbX;
                double LY = state.Gamepad.LeftThumbY;
                double RX = state.Gamepad.RightThumbX;
                double RY = state.Gamepad.RightThumbY;

                StringBuilder command = new StringBuilder();
                command.Append("<M");

                string leftDirection = LY > 0 ? "F" : "B";
                command.Append(leftDirection);
                int leftSpeed = (int)Math.Abs((LY * 255 / 32768));
                command.Append(getPaddedSpeed(leftSpeed));

                string rightDirection = RY > 0 ? "F" : "B";
                command.Append(rightDirection);
                int rightSpeed = (int)Math.Abs((RY * 255 / 32768));
                command.Append(getPaddedSpeed(rightSpeed));

                command.Append(">");

                logger.Debug(command.ToString());
                //TODO: Send command
                //Need to implement command sender

                previousState = state;
                Thread.Sleep(POLLING_RATE);
            }
        }

        private void PollAndSendCameraCommands(Controller controller)
        {
            var previousState = controller.GetState();
            while (controller.IsConnected)
            {
                var state = controller.GetState();
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y))
                {
                    selectedCamera = 0;
                }
                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                {
                    selectedCamera = 1;
                }
                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
                {
                    selectedCamera = 2;
                }
                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.X))
                {
                    selectedCamera = 3;
                }
                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder))
                {
                    selectedCamera = 4;
                }



                previousState = state;
                Thread.Sleep(POLLING_RATE);
            }
        }

        private string getPaddedSpeed(int speed)
        {
            string paddedString = null;

            if (speed < 10) { paddedString = "00" + speed; }
            else if (speed < 100 && speed > 9) { paddedString = "0" + speed; }
            else if (speed > 99) { paddedString = speed + ""; }

            return paddedString;
        }

    }
}
