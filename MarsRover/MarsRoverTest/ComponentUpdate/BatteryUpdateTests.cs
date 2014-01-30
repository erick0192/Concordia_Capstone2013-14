using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover;
using MarsRover.Exceptions;
using NUnit.Framework;

namespace MarsRoverTest.ComponentUpdate
{
    [TestFixture, Category("ComponentUpdateTests")]
    class BatteryUpdateTests
    {
        /*
         * Naming Convention: MethodName_StateUnderTest_ExpectedResults 
         */        

        [Test]
        public void UpdateFromString_InvalidUpdateString_ThrowException()
        {
            Battery b = new Battery(200) { ChargePerc = 20.0f, Current = 23.5f, Temperature = 54.7f };
            string updateString;

            updateString = "<V;57.564,254500.122,70.523>";
            Assert.Throws<InvalidUpdateStringException>(() => b.UpdateFromString(updateString));
            updateString = "<B;57.5624,254500.122,70.523>";
            Assert.Throws<InvalidUpdateStringException>(() => b.UpdateFromString(updateString));
            updateString = "<B;57.564,254500.122,70.523156>";
            Assert.Throws<InvalidUpdateStringException>(() => b.UpdateFromString(updateString));
            updateString = "B;57.564,254500.122,70.5";
            Assert.Throws<InvalidUpdateStringException>(() => b.UpdateFromString(updateString));
            updateString = "<B;57.564,70.523156>";
            Assert.Throws<InvalidUpdateStringException>(() => b.UpdateFromString(updateString));
        }

        [Test]
        public void UpdateFromString_InvalidUpdateString_ObjectUpdated()
        {
            Battery b = new Battery(200) { ChargePerc = 20.0f, Current = 23.5f, Temperature = 54.7f };
            string updateString;

            updateString = "<B;57.564,254500.122,70.523>";
            b.UpdateFromString(updateString);

            Assert.AreEqual(57.564f, b.ChargePerc);
            Assert.AreEqual(564, 254500.122f, b.Current);
            Assert.AreEqual(70.523f, b.Temperature);
        }

        [Test]
        public void GetUpdateString_ValidUpdateStringGenerated_ValidUpdateStringReturned()
        {
            Battery b = new Battery(200) { ChargePerc = 20.0012f, Current = 23.5f, Temperature = 54.7456f };
            Assert.AreEqual(String.Format("<B;{0},{1},{2}>",
                Math.Round(b.ChargePerc, 3), Math.Round(b.Current, 3), Math.Round(b.Temperature, 3)),
                b.GetUpdateString());            
        }
    }
}
