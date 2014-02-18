using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover.Commands;

namespace Rover.Commands
{
    class KeepAliveCommand : ICommand
    {
        private const string KEEP_ALIVE_SIGNAL = "<K>";

        private MicrocontrollerSingleton microcontroller;

        public KeepAliveCommand()
        {
            microcontroller = MicrocontrollerSingleton.Instance;
        }

        public void Execute()
        {
            microcontroller.WriteMessage(KEEP_ALIVE_SIGNAL);
        }

        public void UnExecute()
        {
            this.Execute();
        }
    }
}
