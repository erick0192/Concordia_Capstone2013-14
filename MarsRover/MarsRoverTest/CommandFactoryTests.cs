using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rover;
using Rover.Commands;
using MarsRover.Commands;

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
        public void CreateCommand_ValidLeftMovementString_ReturnsMovementCommand()
        {
            string commandString = "<LF255F255F255>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(commandString);

            Assert.IsInstanceOf(typeof(MovementCommand), command);
        }

        [Test]
        public void CreateCommand_ValidRightMovementString_ReturnsMovementCommand()
        {
            string commandString = "<RF255F255F255>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(commandString);

            Assert.IsInstanceOf(typeof(MovementCommand), command);
        }

        [Test]
        public void CreateCommand_ValidCameraString_ReturnsCameraCommand()
        {
            string cameraString = "<C1O>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(cameraString);

            Assert.IsInstanceOf(typeof(CameraCommand), command);
        }

        [Test]
        public void CreateCommand_ValidPanString_ReturnsPanCommand()
        {
            string panString = "<P1200>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(panString);

            Assert.IsInstanceOf(typeof(PanCommand), command);
        }

        [Test]
        public void CreateCommand_ValidTiltString_ReturnsTiltCommand()
        {
            string tiltString = "<T1000>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(tiltString);

            Assert.IsInstanceOf(typeof(TiltCommand), command);
        }

        [Test]
        public void CreateCommand_DuplicateValidCommandStrings_ReturnsNullCommandOnSecondInstantiation()
        {
            string commandString = "<LF255F255F255>";
            CommandFactory factory = new CommandFactory();
            ICommand command = factory.CreateCommand(commandString);

            Assert.IsNotInstanceOf(typeof(NullCommand), command, "Valid string inputted, should return a non-null command");
             
            command = factory.CreateCommand(commandString);

            Assert.IsInstanceOf(typeof(NullCommand), command);
        }

        [Test]
        public void CreateCommand_DifferentValidMovementCommandStrings_ReturnsTwoMovementCommands()
        {
            string commandString = "<LF255F255F255>";
            string commandString2 = "<LF200F200F200>";
            CommandFactory factory = new CommandFactory();

            ICommand command = factory.CreateCommand(commandString);
            Assert.IsInstanceOf(typeof(MovementCommand), command);

            command = factory.CreateCommand(commandString2);
            Assert.IsInstanceOf(typeof(MovementCommand), command);
        }

        [Test]
        public void CreateCommand_ValidLeftMovementValidRightMovementCommandStrings_ReturnsTwoMovementCommands()
        {
            string commandString = "<LF255F255F255>";
            string commandString2 = "<RF000F000F000>";
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
            string commandString1 = "<RF255F255F255>";
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
