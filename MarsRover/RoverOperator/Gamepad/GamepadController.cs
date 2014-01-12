using System;
using System.Threading;
using NLog;
using SharpDX;
using SharpDX.XInput;
using System.Collections.Generic;
using System.Text;

namespace RoverOperator.Gamepad
{
    class GamepadController
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private const int POLLING_RATE = 100; //milliseconds

        public GamepadController()
        {
            //TODO: Detect new controllers after start ?
            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };
            StartPollingAndSendingCommands(controllers);
        }

        public void StartPollingAndSendingCommands(Controller[] controllers)
        {
            foreach (var controller in controllers)
            {
                if (controller.IsConnected)
                {
                    logger.Debug("Found a XInput controller available");
                    Thread t = new Thread(() => Poll(controller));
                    t.IsBackground = true;
                    t.Start();
                }
            }
        }

        private void Poll(Controller controller)
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
                string leftSpeed = (int)Math.Abs((LY * 255 / 32768)) + "";
                command.Append(leftSpeed);

                string rightDirection = RY > 0 ? "F" : "B";
                command.Append(rightDirection);
                string rightSpeed = (int)Math.Abs((RY * 255 / 32768)) + "";
                command.Append(rightSpeed);

                command.Append(">");
                
                //logger.Debug(command);
                //TODO: Send command

                previousState = state;
                Thread.Sleep(POLLING_RATE);
            }
        }

    }
}
