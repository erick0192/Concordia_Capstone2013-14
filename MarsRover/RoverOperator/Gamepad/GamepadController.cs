using System;
using System.Threading;
using NLog;
using SharpDX;
using SharpDX.XInput;
using System.Collections.Generic;
using System.Text;
using MarsRover;
using System.Collections.Concurrent;

namespace RoverOperator.Gamepad
{
    class GamepadController
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private const int POLLING_RATE = 200; //milliseconds
        private const int STATE_CHECK_INTERVAL = 50;
        private const int BUTTON_PRESS_SAFE_INTERVAL = 1000;
        Dictionary<int, CameraState> cameraStates;

        private bool logging = true;

        protected class CameraState
        {
            public String Pan;
            public int Tilt;
            public bool Active;
        };

        public GamepadController()
        {
            //TODO: Detect new controllers after start ?
            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two) };
            StartPollingAndSendingCommands(controllers);

            cameraStates = new Dictionary<int, CameraState>(5);
            for (int i = 0; i < 5; i++)
            {
                CameraState cs = new CameraState();
                cs.Active = false;
                cs.Pan = "100";
                cs.Tilt = 0;
                cameraStates[i] = cs;
            }
        }

        public void StartPollingAndSendingCommands(Controller[] controllers)
        {
            var commandsQueue = new BlockingCollection<string>();

            if (controllers[0].IsConnected)
            {
                if (logging) logger.Trace("Found a XInput controller available. Starting PollAndSendMovementCommands");
                Thread t = new Thread(() => PollAndSendMovementCommands(controllers[0], commandsQueue));
                t.IsBackground = true;
                t.Start();
            }

            if (controllers[1].IsConnected)
            {
                if (logging) logger.Trace("Found a XInput controller available. Starting PollAndSendCameraCommands");
                Thread t = new Thread(() => PollAndSendCameraCommands(controllers[1], commandsQueue, cameraStates));
                Thread commandsSender = new Thread(() => ProcessCommandQueue(commandsQueue, cameraStates));
                commandsSender.Start();
                t.IsBackground = true;
                t.Start();
            }

        }

        private void PollAndSendMovementCommands(Controller controller, BlockingCollection<string> commands)
        {
            var previousState = controller.GetState();
            int selectedCamera = 1; //Front camera is default
            int angleIncrement = 5;

            while (controller.IsConnected)
            {
                ////////////////////////////////////////////////////////////////////////////Movement Control
                var state = controller.GetState();
                double LX = state.Gamepad.LeftThumbX;
                double LY = state.Gamepad.LeftThumbY;
                double RX = state.Gamepad.RightThumbX;
                double RY = state.Gamepad.RightThumbY;

                //Building commands for left motors
                string leftDirection = LY > 0 ? "F" : "B";
                int leftSpeed = (int)Math.Abs((LY * 255 / 32768));
                    //Minor deadzone settings
                if (leftSpeed < 30) leftSpeed = 0;
                else if (leftSpeed > 230) leftSpeed = 255;

                if (RoverOperator.Content.MotorsViewModel.MotorVMActive)
                {

                    StringBuilder leftCommand = new StringBuilder();
                    leftCommand.Append("<L");
                    string frontLeftMotorCommand = leftDirection + getPaddedInt(leftSpeed * (int)RoverOperator.Content.MotorsViewModel.FrontLeftMotorVM.Power / 100);
                    leftCommand.Append(frontLeftMotorCommand);
                    string middleLeftMotorCommand = leftDirection + getPaddedInt(leftSpeed * (int)RoverOperator.Content.MotorsViewModel.MiddleLeftMotorVM.Power / 100);
                    leftCommand.Append(middleLeftMotorCommand);
                    string backLeftMotorCommand = leftDirection + getPaddedInt(leftSpeed * (int)RoverOperator.Content.MotorsViewModel.BackLeftMotorVM.Power / 100);
                    leftCommand.Append(backLeftMotorCommand);
                    leftCommand.Append(">");
                    CommandSender.Instance.UpdateCommand(leftCommand.ToString());
                    if (logging) logger.Trace(leftCommand.ToString());
                }

                //Building commands for left motors
                string rightDirection = RY > 0 ? "F" : "B";
                int rightSpeed = (int)Math.Abs((RY * 255 / 32768));
                    //Minor deadzone settings
                if (rightSpeed < 30) rightSpeed = 0;
                else if (rightSpeed > 230) rightSpeed = 255;

                if (RoverOperator.Content.MotorsViewModel.MotorVMActive)
                {
                    StringBuilder rightCommand = new StringBuilder();
                    rightCommand.Append("<R");
                    string frontRightMotorCommand = rightDirection + getPaddedInt(rightSpeed * (int)RoverOperator.Content.MotorsViewModel.FrontRightMotorVM.Power / 100);
                    rightCommand.Append(frontRightMotorCommand);
                    string middleRightMotorCommand = rightDirection + getPaddedInt(rightSpeed * (int)RoverOperator.Content.MotorsViewModel.MiddleRightMotorVM.Power / 100);
                    rightCommand.Append(middleRightMotorCommand);
                    string backRightMotorCommand = rightDirection + getPaddedInt(rightSpeed * (int)RoverOperator.Content.MotorsViewModel.BackRightMotorVM.Power / 100);
                    rightCommand.Append(backRightMotorCommand);
                    rightCommand.Append(">");
                    CommandSender.Instance.UpdateCommand(rightCommand.ToString());
                    if (logging) logger.Trace(rightCommand.ToString());
                }


                ////////////////////////////////////////////////////////////////////////////Camera Control

                //Selecting camera to pan/tilt
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y))
                {
                    selectedCamera = 1;
                }
                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                {
                    selectedCamera = 2;
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

                    //Send command
                    if (userSelected)
                    {
                        commands.Add(command.ToString());
                    }
                }

                bool panning = false;
                bool tilting = false;

                //Actually panning/tilting
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft)) //Left Pan
                {
                    panning = true;
                    cameraStates[selectedCamera].Pan = "000";
                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight)) //Right Pan
                {
                    panning = true;
                    cameraStates[selectedCamera].Pan = "200";
                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp)) //Up Tilt
                {
                    if (cameraStates[selectedCamera].Tilt >= 90 - angleIncrement)
                        cameraStates[selectedCamera].Tilt = 90;
                    else
                        cameraStates[selectedCamera].Tilt += angleIncrement;
                    tilting = true;
                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown)) //Down Tilt
                {
                    if (cameraStates[selectedCamera].Tilt <= angleIncrement)
                        cameraStates[selectedCamera].Tilt = 0;
                    else
                        cameraStates[selectedCamera].Tilt -= angleIncrement;
                    tilting = true;
                }

                //Sending commands
                if (panning)
                {
                    string command = "<P" + selectedCamera + cameraStates[selectedCamera].Pan + ">";
                    if (logging) logger.Trace(command.ToString());
                    //commands.Add(command.ToString());
                    CommandSender.Instance.UpdateCommand(command.ToString());
                }
                else
                {
                    string command = "<P" + selectedCamera + "100>";
                    if (logging) logger.Trace(command.ToString());
                   // commands.Add(command.ToString());
                    CommandSender.Instance.UpdateCommand(command.ToString());
                }

                if (tilting)
                {
                    string command = "<T" + selectedCamera + getPaddedInt(cameraStates[selectedCamera].Tilt) + ">";
                    //commands.Add(command.ToString());
                    CommandSender.Instance.UpdateCommand(command.ToString());
                }

                previousState = state;
                Thread.Sleep(POLLING_RATE);

            }
        }

        private void PollAndSendCameraCommands(Controller controller, BlockingCollection<string> commands, Dictionary<int, CameraState> cameraStates)
        {

            var previousState = controller.GetState();
            int selectedCamera = 0; //Default camera is broom
            int angleIncrement = 5;
            
            while (controller.IsConnected)
            {
                var state = controller.GetState();
                //Selecting camera to pan/tilt
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
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
                    if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
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
                        commands.Add(command.ToString());
                    }
                }

                bool panning = false;
                bool tilting = false;

                //Actually panning/tilting
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft)) //Left Pan
                {
                    panning = true;
                    cameraStates[selectedCamera].Pan = "000";
                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight)) //Right Pan
                {
                    panning = true;
                    cameraStates[selectedCamera].Pan = "200";
                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp)) //Up Tilt
                {
                    if (cameraStates[selectedCamera].Tilt >= 90 - angleIncrement)
                        cameraStates[selectedCamera].Tilt = 90;
                    else
                        cameraStates[selectedCamera].Tilt += angleIncrement;
                    tilting = true;
                }

                else if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown)) //Down Tilt
                {
                    if (cameraStates[selectedCamera].Tilt <= angleIncrement)
                        cameraStates[selectedCamera].Tilt = 0;
                    else
                        cameraStates[selectedCamera].Tilt -= angleIncrement;
                    tilting = true;
                }

                //Sending commands
                if (panning)
                {
                    string command = "<P" + selectedCamera + cameraStates[selectedCamera].Pan + ">";
                    //commands.Add(command.ToString());
                    CommandSender.Instance.UpdateCommand(command.ToString());
                }
                else
                {
                    string command = "<P" + selectedCamera + "100>";
                    //commands.Add(command.ToString());
                    CommandSender.Instance.UpdateCommand(command.ToString());
                }

                if (tilting)
                {
                    string command = "<T" + selectedCamera + getPaddedInt(cameraStates[selectedCamera].Tilt) + ">";
                   // commands.Add(command.ToString());
                    CommandSender.Instance.UpdateCommand(command.ToString());
                }

                previousState = state;
                Thread.Sleep(POLLING_RATE);
            }
        }

        private void ProcessCommandQueue(BlockingCollection<string> commands, Dictionary<int, CameraState> cameraStates)
        {
            var previousSelectedCamera = -1;
            var timePreviousCameraSelect = DateTime.Now;

            foreach (string command in commands.GetConsumingEnumerable())
            {
                if (command.Contains("<C"))
                {
                    var cameraNumber = getCameraNumberFromToggleCommand(command);
                    if (previousSelectedCamera == cameraNumber && (DateTime.Now - timePreviousCameraSelect).TotalMilliseconds < BUTTON_PRESS_SAFE_INTERVAL)
                    {
                        cameraStates[cameraNumber].Active = !cameraStates[cameraNumber].Active;
                        if (logging) logger.Trace("Ignored command " + command);
                        continue;
                    }
                    else
                    {
                        previousSelectedCamera = cameraNumber;
                        timePreviousCameraSelect = DateTime.Now;
                        CommandSender.Instance.UpdateCommand(command.ToString());
                        if (ViewModelManager.Instance.MainWindowVM.MainVM.ToggleCameraCommand.CanExecute(""+cameraNumber))
                            ViewModelManager.Instance.MainWindowVM.MainVM.ToggleCameraCommand.Execute(""+cameraNumber);
                        Thread.Sleep(STATE_CHECK_INTERVAL);
                        if (logging) logger.Trace(command.ToString());
                    }
                }
                else
                {
                    CommandSender.Instance.UpdateCommand(command.ToString());
                    Thread.Sleep(STATE_CHECK_INTERVAL);
                    if (logging) logger.Trace(command.ToString());
                }
            }

        }

        private int getCameraNumberFromToggleCommand(string command)
        {
            return int.Parse(command.Substring(2, 1));
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
