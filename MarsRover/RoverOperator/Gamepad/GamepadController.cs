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
        private UDPSender udpSender;

        protected class CameraState
        {
            public int Pan;
            public int Tilt;
            public bool Active;
        };

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
                command.Append(getPaddedInt(leftSpeed));

                string rightDirection = RY > 0 ? "F" : "B";
                command.Append(rightDirection);
                int rightSpeed = (int)Math.Abs((RY * 255 / 32768));
                command.Append(getPaddedInt(rightSpeed));

                command.Append(">");
                //TODO: Send command
                //Need to implement command sender
                udpSender.SendStringNow(command.ToString());

                previousState = state;
                Thread.Sleep(POLLING_RATE);
            }
        }

        private void PollAndSendCameraCommands(Controller controller)
        {
            var previousState = controller.GetState();
            int selectedCamera = 0; //Default camera is broom
            int angleIncrement = 5;
            Dictionary<int, CameraState> cameraStates = new Dictionary<int, CameraState>(5);
            for (int i = 0; i < 5; i++) {
                CameraState cs = new CameraState();
                cs.Active = false;
                cs.Pan = 0;
                cs.Tilt = 0;
                cameraStates[i] = cs;
            }


            while (controller.IsConnected)
            {
                var state = controller.GetState();
                //Selecting camera to pan/tilt
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y))
                {
                    selectedCamera = 1;
                }
                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                {
                    selectedCamera = 2;
                }
                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
                {
                    selectedCamera = 3;
                }
                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.X))
                {
                    selectedCamera = 4;
                }
                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder))
                {
                    selectedCamera = 0;
                }

                //De-Activating camera to pan/tilt
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder))
                {
                    string command = null;
                    bool userSelected = false;
                    if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y))
                    {
                        selectedCamera = 1;
                        if (cameraStates[1].Active)
                            command = "<C" + 1 + "F>";
                        else
                            command = "<C" + 1 + "O>";
                        cameraStates[1].Active = !cameraStates[1].Active;
                        userSelected = true;
                    }
                    else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                    {
                        selectedCamera = 2;
                        if (cameraStates[2].Active)
                            command = "<C" + 2 + "F>";
                        else
                            command = "<C" + 2 + "O>";
                        cameraStates[2].Active = !cameraStates[2].Active;
                        userSelected = true;
                    }
                    else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
                    {
                        selectedCamera = 3;
                        if (cameraStates[3].Active) //Dectivating
                            command = "<C" + 3 + "F>";
                        else
                            command = "<C" + 3 + "O>";
                        cameraStates[3].Active = !cameraStates[3].Active;
                        userSelected = true;
                    }
                    else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.X))
                    {
                        selectedCamera = 4;
                        if (cameraStates[4].Active) //Dectivating
                            command = "<C" + 4 + "F>";
                        else
                            command = "<C" + 4 + "O>";
                        cameraStates[4].Active = !cameraStates[4].Active;
                        userSelected = true;
                    }
                    else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder))
                    {
                        selectedCamera = 0;
                        if (cameraStates[0].Active) //Dectivating
                            command = "<C" + 0 + "F>";
                        else
                            command = "<C" + 0 + "O>";
                        cameraStates[0].Active = !cameraStates[0].Active;
                        userSelected = true;
                    }
                    //Send command
                    if (userSelected)
                    {
                        udpSender.SendStringNow(command);
                        logger.Debug(command);
                    }
                }

                //Actually panning/tilting
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft)) //Left Pan
                {
                    if (selectedCamera == 0) //Broom camera - allows an angle of 000-359
                    {
                        if (cameraStates[selectedCamera].Pan >= 359 - angleIncrement)
                            cameraStates[selectedCamera].Pan = 359;
                        else
                            cameraStates[selectedCamera].Pan += angleIncrement;
                    }
                    else //Any other camera - allows an angle of 000-180
                    {
                        if (cameraStates[selectedCamera].Pan >= 180 - angleIncrement)
                            cameraStates[selectedCamera].Pan = 180;
                        else
                            cameraStates[selectedCamera].Pan += angleIncrement;
                    }

                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight)) //Right Pan
                {
                    if (cameraStates[selectedCamera].Pan <= angleIncrement)
                        cameraStates[selectedCamera].Pan = 0;
                    else
                        cameraStates[selectedCamera].Pan -= angleIncrement;
                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp)) //Up Tilt
                {
                    if (cameraStates[selectedCamera].Tilt >= 90 - angleIncrement)
                        cameraStates[selectedCamera].Tilt = 90;
                    else
                        cameraStates[selectedCamera].Tilt += angleIncrement;
                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown)) //Down Tilt
                {
                    if (cameraStates[selectedCamera].Tilt <= angleIncrement)
                        cameraStates[selectedCamera].Tilt = 0;
                    else
                        cameraStates[selectedCamera].Tilt -= angleIncrement;
                }

                //Sending commands
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft) || state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight))
                {
                    string command = "<P" + selectedCamera + getPaddedInt(cameraStates[selectedCamera].Pan) + ">";
                    udpSender.SendStringNow(command);
                    logger.Debug(command);
                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp) || state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown))
                {
                    string command = "<T" + selectedCamera + getPaddedInt(cameraStates[selectedCamera].Tilt) + ">";
                    udpSender.SendStringNow(command);
                    logger.Debug(command);
                }

                previousState = state;
                Thread.Sleep(POLLING_RATE);
            }
        }

        private string getPaddedInt(int toPad)
        {
            string paddedString = null;

            if (toPad < 10) { paddedString = "00" + toPad; }
            else if (toPad < 100 && toPad > 9) { paddedString = "0" + toPad; }
            else if (toPad > 99) { paddedString = toPad + ""; }

            return paddedString;
        }

    }
}
