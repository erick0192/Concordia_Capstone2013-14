using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rover;
using MarsRover.Commands;
using Rover.Commands;
//using Rover.Commands;

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
        public void CreateMovementCommand_ValidLeftFullForward_InitializesProperly()
        {
            string rawCommand = "<LF255F255F255>";
            MovementCommand command = new MovementCommand(rawCommand);

            Assert.AreEqual(command.MotorSide, "L", "Should be \"L\"");
            Assert.AreEqual(command.Motor1Speed, 255, "Should be 255");
            Assert.AreEqual(command.Motor2Speed, 255, "Should be 255");
            Assert.AreEqual(command.Motor3Speed, 255, "Should be 255");
            Assert.AreEqual(command.Motor1Direction, "F", "Should be \"F\"");
            Assert.AreEqual(command.Motor2Direction, "F", "Should be \"F\"");
            Assert.AreEqual(command.Motor3Direction, "F", "Should be \"F\"");

        }

        [Test]
        public void CreateMovementCommand_ValidRightFullForward_InitializesProperly()
        {
            string rawCommand = "<RF255F255F255>";
            MovementCommand command = new MovementCommand(rawCommand);

            Assert.AreEqual(command.MotorSide, "R", "Should be \"R\"");
            Assert.AreEqual(command.Motor1Speed, 255, "Should be 255");
            Assert.AreEqual(command.Motor2Speed, 255, "Should be 255");
            Assert.AreEqual(command.Motor3Speed, 255, "Should be 255");
            Assert.AreEqual(command.Motor1Direction, "F", "Should be \"F\"");
            Assert.AreEqual(command.Motor2Direction, "F", "Should be \"F\"");
            Assert.AreEqual(command.Motor3Direction, "F", "Should be \"F\"");

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
        public void CreateMovementCommand_InvalidSideIdentifier_ThrowsArgumentException()
        {
            string rawCommand = "<XF255F255F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);
        }

        [Test]
        public void CreateMovementCommand_InvalidMotor1Direction_ThrowsArgumentException()
        {
            string rawCommand = "<RX255F255F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);
        }

        [Test]
        public void CreateMovementCommand_InvalidMotor2Direction_ThrowsArgumentException()
        {
            string rawCommand = "<RF255X255F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);
        }

        [Test]
        public void CreateMovementCommand_InvalidMotor3Direction_ThrowsArgumentException()
        {
            string rawCommand = "<RF255F255X255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);
        }

        [Test]
        public void CreateMovementCommand_Motor1SpeedHigherThanMax_ThrowsArgumentException()
        {
            string rawCommand = "<LF999F255F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);

        }

        [Test]
        public void CreateMovementCommand_Motor2SpeedHigherThanMax_ThrowsArgumentException()
        {
            string rawCommand = "<LF255F999F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);

        }

        [Test]
        public void CreateMovementCommand_Motor3SpeedHigherThanMax_ThrowsArgumentException()
        {
            string rawCommand = "<LF255F255F999>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);

        }

        [Test]
        public void CreateMovementCommand_Motor1SpeedLowerThanMin_ThrowsArgumentException()
        {
            string rawCommand = "<LF-50F255F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);

        }

        [Test]
        public void CreateMovementCommand_Motor2SpeedLowerThanMin_ThrowsArgumentException()
        {
            string rawCommand = "<LF255F-50F255>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);

        }

        [Test]
        public void CreateMovementCommand_Motor3SpeedLowerThanMin_ThrowsArgumentException()
        {
            string rawCommand = "<LF255F255F-50>";

            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    MovementCommand command = new MovementCommand(rawCommand);
                }
            , "Command sent: " + rawCommand);

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
        public void CreatePanCommand_PanAngleGreaterThanMax_ThrowsArgumentException()
        {
            string rawCommand = "<P1201>";
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
