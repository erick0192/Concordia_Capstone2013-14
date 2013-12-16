using System;
using System.Threading;
using NLog;
using SharpDX;
using SharpDX.XInput;
using System.Collections.Generic;

namespace MarsRoverClient.Gamepad
{
    class GamepadController
    {
        //Straight up from SharpDX docs
        private Logger logger = LogManager.GetCurrentClassLogger();
        private const int POLLING_RATE = 100; //milliseconds
        private const int LEFT_THUMB_DEADZONE = 7849;

        public GamepadController()
        {
            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

            StartPollingAndSendingCommands(controllers);
        }

        public void StartPollingAndSendingCommands(Controller[] controllers)
        {
            foreach (var controller in controllers)
            {
                if (controller.IsConnected)
                {
                    logger.Debug("Starting thread for Controller {0}", controller.GetHashCode());
                    Thread t = new Thread(() => Poll(controller));
                    t.Start();
                }
            }
        }

        private void Poll(Controller controller)
        {
            logger.Debug("Found a XInput controller available");

            var previousState = controller.GetState();
            while (controller.IsConnected)
            {
                var state = controller.GetState();
                if (previousState.PacketNumber != state.PacketNumber) {
                    //http://msdn.microsoft.com/en-us/library/windows/desktop/ee417001(v=vs.85).aspx#dead_zone
                    int LX = state.Gamepad.LeftThumbX;
                    int LY = state.Gamepad.LeftThumbY;

                    double magnitude = Math.Sqrt(LX * LX + LY * LY);
                    double normalizedLX = LX / magnitude;
                    double normalizedLY = LY / magnitude;
                    double normalizedMagnitude = 0;

                    if (magnitude > LEFT_THUMB_DEADZONE)
                    {
                        if (magnitude > 32767)
                            magnitude = 32767;
                        normalizedMagnitude = magnitude / (32767 - LEFT_THUMB_DEADZONE);
                    }
                    else
                    {
                        magnitude = 0.0;
                        normalizedMagnitude = 0.0;
                    }

                    if (normalizedMagnitude > 0) //For sensitive gamepads
                    {
                        string xCommand = normalizedLX > 0 ? "L" : "R";
                        string yCommand = normalizedLY > 0 ? "W" : "S";
                        //Speed is normalizedMagnitude in [0,1]
                        logger.Debug("Go " + xCommand + " and " + yCommand); //Send by TCP instead
                    }

                    previousState = state;
                }
                Thread.Sleep(POLLING_RATE);
            }
        }
    }
}
