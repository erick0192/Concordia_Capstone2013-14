using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MarsRover;
using MarsRover.Exceptions;

namespace MarsRoverTest.ComponentUpdate
{
    /* 
     * Naming Convention: MethodName_StateUnderTest_ExpectedResults 
     */

    [TestFixture, Category("MotorUpdateTests")]
    class MotorUpdateTests
    {
        [Test]
        public void GetLocationFromUpdateString_InvalidUpdateString_ThrowException()
        {
            Assert.Throws<InvalidUpdateStringException>(() => Motor.GetLocationFromUpdateString("<MR;MR,23,45.0>"));
            Assert.Throws<InvalidUpdateStringException>(() => Motor.GetLocationFromUpdateString("<MR;L,27.0,3>"));
            Assert.Throws<InvalidUpdateStringException>(() => Motor.GetLocationFromUpdateString("<MR;M,,>"));
        }

        [Test]
        public void GetLocationFromUpdateString_ValidUpdateString_ValidMotorLocation()
        {

        }
    }
}
