using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MarsRover;
using Rover.Commands;

namespace Rover
{
    class WatchDog : WatchDogCore
    {
        #region Attributes

        private const string EMERGENCY_LEFT_STOP = "<LF000F000F000>";
        private const string EMERGENCY_RIGHT_STOP = "<RF000F000F000>";
        
        private int _sleepPeriod = 500;

        private MovementCommand emergencyLeftStopCommand;
        private MovementCommand emergencyRightStopCommand;
        private KeepAliveCommand keepAliveCommand;

        #endregion

        #region Constructor

        public WatchDog()
        {
            keepAliveCommand = new KeepAliveCommand();

            Thread keepAlive = new Thread(() => KeepAlive());
            keepAlive.Start();
        }

        #endregion

        #region Methods

        private void KeepAlive()
        {
            while (true)
            {
                keepAliveCommand.Execute();

                if (!allowCommand())
                {
                    //Console.WriteLine("***Emergency stop enabled***");
                    if (emergencyLeftStopCommand == null)
                    {
                        emergencyLeftStopCommand = new MovementCommand(EMERGENCY_LEFT_STOP);
                    }

                    if (emergencyRightStopCommand == null)
                    {
                        emergencyRightStopCommand = new MovementCommand(EMERGENCY_RIGHT_STOP);
                    }

                    emergencyLeftStopCommand.Execute();
                    emergencyRightStopCommand.Execute();
                }

                Thread.Sleep(_sleepPeriod);
            }
        }

        #endregion
    }
}
