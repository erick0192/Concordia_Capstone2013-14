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
     * Naming Convention: MethodBeingTested_StateUnderTest_ExpectedResults
     * 
     */

    [TestFixture]
    class CommandFactoryTests
    {
        [Test]
        public void CreateCommand_ValidMovementString_ReturnsMovementCommand()
        {
            string commandString = "<MF255F255>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(commandString);

            Assert.IsInstanceOf(typeof(MovementCommand), command, "Should return movement command");

           
        }

        [Test]
        public void CreateCommand_DuplicateValidCommandStrings_ReturnsNullCommandOnSecondInstantiation()
        {
            string commandString = "<MF255F255>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(commandString);

            Assert.IsNotInstanceOf(typeof(NullCommand), command, "Valid string inputted, should return a non-null command");

            command = factory.CreateCommand(commandString);

            Assert.IsInstanceOf(typeof(NullCommand), command, "Should return a null command");
        }

        [Test]
        public void CreateCommand_DifferentValidCommandStrings_ReturnsTwoMovementCommands()
        {
            string commandString = "<MF255F255>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(commandString);

            Assert.IsInstanceOf(typeof(MovementCommand), command, "Should return movement command");

            commandString = "<MF000F000>";
            command = factory.CreateCommand(commandString);

            Assert.IsInstanceOf(typeof(MovementCommand), command, "Should return new movement command");
        }
    }
}
