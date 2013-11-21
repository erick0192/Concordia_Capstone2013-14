using System;
using NLog;
using SharpDX.DirectInput;
using System.Collections.Generic;

namespace MarsRoverClient.Gamepad
{
    class GamepadController
    {
        //Straight up from SharpDX docs
        private DirectInput directInput;
        private List<Joystick> gamepads;
        private Logger logger;

        public GamepadController()
        {
            logger = LogManager.GetCurrentClassLogger();
            directInput = new DirectInput();
            gamepads = new List<Joystick>(2);

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                var gamepad = new Joystick(directInput, deviceInstance.InstanceGuid);
                gamepad.Properties.BufferSize = 128;
                gamepads.Add(gamepad);
                logger.Trace("Added Gamepad with Guid: " + deviceInstance.InstanceGuid);
            }
        }

        public void StartPollingAndSendingCommands()
        {
            foreach (var gp in gamepads)
            {
                gp.Acquire();
            }

            //TODO: Start in a thread for each gamepad
            while (true)
            {
                foreach (var gp in gamepads)
                {
                    gp.Poll();
                    var datas = gp.GetBufferedData();
                    foreach (var state in datas)
                        logger.Trace(state);
                }
            }
        }
    }
}
