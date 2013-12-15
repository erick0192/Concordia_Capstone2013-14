using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RobotSoftware;

namespace RobotSoftwareUnitTests
{

    /* 
     * Unit tests for the CommandFactory Class
     * 
     * Naming Convention: MethodName_StateUnderTest_ExpectedResults
     * 
     */

    [TestFixture, Ignore]
    class CommandFactoryTests
    {
        [Test, Ignore]
        public void CreateCommand_ValidMovementCommand_ReturnsMovementCommand()
        {
            string commandString = "<MF255F255>";
            CommandFactory factory = new CommandFactory();
            
            ICommand command = factory.CreateCommand(commandString);

           
        }
    }
}
