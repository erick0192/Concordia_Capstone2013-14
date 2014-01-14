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
     * Naming Convention: MethodName_StateUnderTest_ExpectedResults
     * 
     */

    [TestFixture, Category("CommandTests")]
    class MovementCommandTests
    {
        [Test]
        public void InitializeMovementCommand_ValidForwardMovementString_InitializesProperly()
        {
            string rawCommand = "<MF255F255>";
            MovementCommand command = new MovementCommand(rawCommand);

            Assert.AreEqual(command.GetLeftSpeed(), 255, "Should be 255");
            Assert.AreEqual(command.GetRightSpeed(), 255, "Should be 255");
            Assert.AreEqual(command.GetLeftDirection(), 'F', "Should be 'F'");
            Assert.AreEqual(command.GetRightDirection(), 'F', "Should be 'F'");
        }

        [Test]
        public void InitializeCameraCommand_ValidCam1OffString_InitializesProperly()
        {
            string rawCommand = "<C1F>";
            CameraCommand command = new CameraCommand(rawCommand);

            Assert.AreEqual(command.getCamIndex(), 1);
            Assert.AreEqual(command.getCamStatus(), false);
        }

        [Test]
        public void InitializeCameraCommand_ValidCam2OffString_InitializesProperly()
        {
            string rawCommand = "<C2O>";
            CameraCommand command = new CameraCommand(rawCommand);

            Assert.AreEqual(command.getCamIndex(), 2);
            Assert.AreEqual(command.getCamStatus(), true);
        }
    }
}
