using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarsRover.Commands;

namespace Rover.Commands
{
    public class CommandFactory
    {

        // Used to keep track of the previous commands we've received. Keeps track of the raw string of for each type of command.
        private Dictionary<string, string> commandHistory;
        

        public CommandFactory()
        {
            commandHistory = new Dictionary <string, string>();

        }

        public ICommand CreateCommand(string unparsedCommand)
        {
            string ID = getCommandID(unparsedCommand);

            try
            {
                if (repeatedCommand(unparsedCommand, ID))
                {
                    return new NullCommand();
                }

                else if (ID == CommandMetadata.Movement.LeftIdentifier || ID == CommandMetadata.Movement.RightIdentifier)
                {
                    return new MovementCommand(unparsedCommand);
                }

                else if (ID == CommandMetadata.Camera.Identifier)
                {
                    return new CameraCommand(unparsedCommand);
                }
                else if (ID == CommandMetadata.Pan.Identifier)
                {
                    return new PanCommand(unparsedCommand);
                }
                else if (ID == CommandMetadata.Tilt.Identifier)
                {
                    return new TiltCommand(unparsedCommand);
                }
                //...Add other commands here

                else
                {
                    //If logging is implemented, log what was received here.
                    return new NullCommand();
                }
            }
            catch (Exception e)
            {
                //log error here
                Console.WriteLine(e.Message);

                return new NullCommand();
            }

        }

        private string getCommandID(string unparsedCommanmd)
        {
            return unparsedCommanmd.Substring(CommandMetadata.IdIndex, CommandMetadata.IdLength);
        }

        private bool repeatedCommand(string unparsedCommand, string ID)
        {
            string lastCommand = "";

            //Special case needed for camera on/off commands since we want to remember the last command sent to each camera.
            //This means that we want to store the previous command for each camera as opposed 
            if (ID == CommandMetadata.Camera.Identifier)
            {
                ID = ID + unparsedCommand.Substring(CommandMetadata.Camera.NumberIndex, CommandMetadata.Camera.NumberLength);
            }

            if (commandHistory.ContainsKey(ID))
            {
                lastCommand = commandHistory[ID];
                if (unparsedCommand == lastCommand)
                {
                    return true;
                }
                else
                {
                    commandHistory[ID] = unparsedCommand;
                    return false;
                }
            }
            else
            {
                commandHistory.Add(ID, unparsedCommand);
                return false;
            }

        }

    }
}
