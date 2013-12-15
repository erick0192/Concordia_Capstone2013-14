using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotSoftware
{
    public class CommandFactory
    {

        public CommandFactory()
        {

        }


        public ICommand CreateCommand(string unparsedCommand)
        {
            char ID = getID(unparsedCommand);

            if (ID == CommandMetadata.Movement.IdentifierCharacter)
            {
                return new MovementCommand(unparsedCommand);
            }
            //...Add other commands here
            return null;

        }

        private char getID(string unparsedCommanmd)
        {
            return unparsedCommanmd[CommandMetadata.CommandIdentifierIndex];
        }

    }
}
