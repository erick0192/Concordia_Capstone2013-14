using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotSoftware
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

            if (repeatedCommand(unparsedCommand, ID))
            {
                return new NullCommand();
            }

            else if (ID == CommandMetadata.Movement.IdentifierCharacter)
            {
                return new MovementCommand(unparsedCommand);
            }

            //...Add other commands here

            else
            {
                //If logging is implemented, log what was received here.
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
