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
    }
}
