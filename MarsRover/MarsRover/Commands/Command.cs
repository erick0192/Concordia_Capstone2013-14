using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Commands
{
    public interface ICommand
    {

        //Expected commands to be implemented:
        //Movement commands
        //Arm commands
        //Camera commands
        //Servo commands
        //End effector commands
        //Collection bin commands

        void Execute();
        void UnExecute();
    }

    public class NullCommand : ICommand
    {
        public NullCommand(string unparsedText)
        {
            //return; //do nothing
        }

        public NullCommand()
        {
            //return;
        }

        public void Execute()
        {
            return;
        }

        public void UnExecute()
        {
            return;
        }
    }    
}
