using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Rover.Commands;

namespace MarsRover
{
    class WatchDog : WatchDogCore
    {
        #region Attributes

        private const string EMERGENCY_STOP = "<MF000F000>";
        
        private int _sleepPeriod = 500;

        private MovementCommand emergencyStopCommand;
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
                    if (emergencyStopCommand == null)
                    {
                        emergencyStopCommand = new MovementCommand(EMERGENCY_STOP);
                    }

                    emergencyStopCommand.Execute();
                }

                Thread.Sleep(_sleepPeriod);
            }
        }

        #endregion
    }
}
