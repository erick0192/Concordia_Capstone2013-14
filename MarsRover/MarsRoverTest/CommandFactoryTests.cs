using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rover;

namespace MarsRoverTest
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

            Assert.IsInstanceOf(typeof(MovementCommand), command);

           
        }

        [Test]
        public void CreateCommand_DuplicateValidCommandStrings_ReturnsNullCommandOnSecondInstantiation()
        {
            string commandString = "<MF255F255>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(commandString);

            Assert.IsNotInstanceOf(typeof(NullCommand), command, "Valid string inputted, should return a non-null command");
             
            command = factory.CreateCommand(commandString);

            Assert.IsInstanceOf(typeof(NullCommand), command);
        }

        [Test]
        public void CreateCommand_DifferentValidMovementCommandStrings_ReturnsTwoMovementCommands()
        {
            string commandString = "<MF255F255>";
            string commandString2 = "<MF000F000>";
            CommandFactory factory = new CommandFactory();

            ICommand command = factory.CreateCommand(commandString);
            Assert.IsInstanceOf(typeof(MovementCommand), command);

            command = factory.CreateCommand(commandString2);
            Assert.IsInstanceOf(typeof(MovementCommand), command);
        }

        [Test]
        public void CreateCommand_DifferentValidCameraCommandStrings_ReturnsTwoCameraCommands()
        {
            string commandString = "<C1O>";
            string commandString2 = "<C1F>";
            CommandFactory factory = new CommandFactory();

            ICommand command = factory.CreateCommand(commandString);
            Assert.IsInstanceOf(typeof(CameraCommand), command);

            command = factory.CreateCommand(commandString2);
            Assert.IsInstanceOf(typeof(CameraCommand), command);
        }

        [Test] 
        public void CreateCommand_SeperatedDuplicateCommands_ReturnsNullCommandOnThirdInstantiation()
        {
            string commandString1 = "<MF255F255>";
            string commandString2 = "<C1F>";
            CommandFactory factory = new CommandFactory();

            ICommand command = factory.CreateCommand(commandString1);
            Assert.IsInstanceOf(typeof(MovementCommand), command);

            command = factory.CreateCommand(commandString2);
            Assert.IsInstanceOf(typeof(CameraCommand), command);

            command = factory.CreateCommand(commandString1);
            Assert.IsInstanceOf(typeof(NullCommand), command, "Duplicate commands should return a null command");
            
        }
    }
}
