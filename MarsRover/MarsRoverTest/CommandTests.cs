using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rover;

namespace MarsRoverTest
{
     /* 
     * Unit tests for commands
     * 
     * Naming Convention: MethodName_StateUnderTest_ExpectedResults
     * 
     */

    [TestFixture, Category("MovementCommandTests")]
    class MovementCommandTests
    {
        [Test]
        public void CreateMovementCommand_ValidForwardLeftForwardRightMovementString_InitializesProperly()
        {
            string rawCommand = "<MF255F255>";
            MovementCommand command = new MovementCommand(rawCommand);

            Assert.AreEqual(command.LeftSpeed, 255, "Should be 255");
            Assert.AreEqual(command.RightSpeed, 255, "Should be 255");
            Assert.AreEqual(command.LeftDirection, "F", "Should be \"F\"");
            Assert.AreEqual(command.RightDirection, "F", "Should be \"F\"");
        }

        [Test]
        public void CreateMovementCommand_NullStringInserted_ThrowsArgumentNullException()
        {
            string rawCommand = null;
            Assert.Throws<System.ArgumentNullException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            );
        }

        [Test]
        public void CreateMovementCommand_InvalidLeftMovementDirection_ThrowsArgumentException()
        {
            string rawCommand = "<MX255F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: <MX255F255>");
        }

        [Test]
        public void CreateMovementCommand_InvalidRightMovementDirection_ThrowsArgumentException()
        {
            string rawCommand = "<MF255X255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: <MF255X255>");
        }

        [Test]
        public void CreateMovementCommand_LeftSpeedHigherThanMax_ThrowsArgumentException()
        {
            string rawCommand = "<MF350F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: <MF350F255>");

        }

        [Test]
        public void CreateMovementCommand_RightSpeedHigherThanMax_ThrowsArgumentException()
        {
            string rawCommand = "<MF255F350>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: <MF255F350>");

        }

        [Test]
        public void CreateMovementCommand_InvalidCommandIdentifier_ThrowsArgumentException()
        {
            string rawCommand = "<CF255F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            );

        }

        [Test]
        public void CreateMovementCommand_GarbageString_ThrowsArgumentException()
        {
            string rawCommand = "K334/.e;fe";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            );
        }
    }

    [TestFixture, Category("CameraCommandTests")]
    class CameraCommandTests
    {
        [Test]
        public void CreateCameraCommand_ValidCam1OffString_InitializesProperly()
        {
            string rawCommand = "<C1F>";
            CameraCommand command = new CameraCommand(rawCommand);

            Assert.AreEqual(command.CameraIndex, 1);
            Assert.AreEqual(command.CameraStatus, false);
        }

        [Test]
        public void CreateCameraCommand_ValidCam2OffString_InitializesProperly()
        {
            string rawCommand = "<C2O>";
            CameraCommand command = new CameraCommand(rawCommand);

            Assert.AreEqual(command.CameraIndex, 2);
            Assert.AreEqual(command.CameraStatus, true);
        }
    }

    [TestFixture, Category("PanCommandTests")]
    class PanCommandTests
    {
        [Test]
        public void CreatePanCommand_ValidInputString_InitializesProperly()
        {
            string rawCommand = "<P1000>";
            int actualCamIndex = 1;
            int actualPanAngle = 0;

            PanCommand command = new PanCommand(rawCommand);
            Assert.AreEqual(command.CameraIndex, actualCamIndex);
            Assert.AreEqual(command.Angle, actualPanAngle);
        }

        [Test]
        public void CreatePanCommand_PanAngleGreaterThan359_ThrowsArgumentException()
        {
            string rawCommand = "<P1360>";
            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    PanCommand command = new PanCommand(rawCommand);
                }
            );
        }

    }

    [TestFixture, Category("TiltCommandTests")]
    class TiltCommandTests
    {
        [Test]
        public void CreateTiltCommand_ValidInputString_InitializesProperly()
        {
            string rawCommand = "<T1090>";
            int expectedCameraIndex = 1;
            int expectedTiltAngle = 90;

            TiltCommand command = new TiltCommand(rawCommand);
            Assert.AreEqual(expectedCameraIndex, command.CameraIndex);
            Assert.AreEqual(expectedTiltAngle, command.Angle);
        }

        [Test]
        public void CreateTiltCommand_TiltAngleMoreThan90_ThrowsArgumentException()
        {
            string rawCommand = "<T0091>";
            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    TiltCommand command = new TiltCommand(rawCommand);
                }
            );
        }
    }
}
