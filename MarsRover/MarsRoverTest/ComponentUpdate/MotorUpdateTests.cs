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

    [TestFixture, Category("ComponentUpdateTests")]
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
            Assert.AreEqual(Motor.Location.BackLeft, Motor.GetLocationFromUpdateString("<MR;B,L,23,45.0>"));
            Assert.AreEqual(Motor.Location.BackRight, Motor.GetLocationFromUpdateString("<MR;B,R,23.23,45.0>"));
            Assert.AreEqual(Motor.Location.FrontLeft, Motor.GetLocationFromUpdateString("<MR;F,L,1,42.0>"));
            Assert.AreEqual(Motor.Location.FrontRight, Motor.GetLocationFromUpdateString("<MR;F,R,2.1,4>"));
            Assert.AreEqual(Motor.Location.MiddleLeft, Motor.GetLocationFromUpdateString("<MR;M,L,111,45.0>"));
            Assert.AreEqual(Motor.Location.MiddleRight, Motor.GetLocationFromUpdateString("<MR;M,R,23.12,45.0>"));
        }

        [Test]
        public void UpdateFromString_InvalidUpdateString_ThrowException()
        {
            //This also tests the IsMatch method
            Motor m = new Motor(Motor.Location.MiddleRight);
            string updateString;

            updateString = "<M;M,R,23.12,45.0>";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
            updateString = "<MR;C,R,23.12,45.0>";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
            updateString = "MR;M,R,23.12,45.0";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
            updateString = "<MR;M,R,23.124453,45.0>";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
            updateString = "<MR;M,R,23.12,45.0145>";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
            updateString = "<MR;M,R,23.12,45.>";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
            updateString = "<MR;M,B,23.12,45.0>";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
            updateString = "<MR;M,R23.12,45.0>";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
            updateString = "<MRM,R,23.12,45.0>";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
            updateString = "<MR;F,R,23.12,45.0>";
            Assert.Throws<InvalidUpdateStringException>(() => m.UpdateFromString(updateString));
        }

        [Test]
        public void UpdateFromString_ValidUpdateString_ObjectUpdated()
        {
            Motor m = new Motor(Motor.Location.MiddleRight) { Current = 23.0f, Temperature = 50.0f };
            string updateString;

            updateString = "<MR;M,R,23.12,45.0>";
            m.UpdateFromString(updateString);

            Assert.AreEqual(23.12f, m.Current);
            Assert.AreEqual(45.0f, m.Temperature);
        }

        [Test]
        public void GetUpdateString_ValidUpdateStringGenerated_ValidUpdateStringReturned()
        {            
            Motor m = new Motor(Motor.Location.MiddleRight) { Current = 23.0239f, Temperature = 50.0423f };

            Assert.AreEqual(String.Format("<MR;M,R,{0},{1}>", Math.Round(m.Current, 3), Math.Round(m.Temperature, 3)), m.GetUpdateString());            
        }
    }
}
