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
    class BatterCellUpdateTests
    {
        /*
         * Naming Convention: MethodName_StateUnderTest_ExpectedResults 
         */

        [Test]
        public void GetCellIDFromUpdateString_ValidUpdateString_CellIDReturned()
        {
            string updateString;

            updateString = "<BC;1,234.3>";
            Assert.AreEqual(1, BatteryCell.GetCellIDFromUpdateString(updateString));
            updateString = "<BC;2,234.3>";
            Assert.AreEqual(2, BatteryCell.GetCellIDFromUpdateString(updateString));
            updateString = "<BC;3,234.3>";
            Assert.AreEqual(3, BatteryCell.GetCellIDFromUpdateString(updateString));
            updateString = "<BC;4,234.3>";
            Assert.AreEqual(4, BatteryCell.GetCellIDFromUpdateString(updateString));
            updateString = "<BC;5,234.3>";
            Assert.AreEqual(5, BatteryCell.GetCellIDFromUpdateString(updateString));
            updateString = "<BC;6,234.3>";
            Assert.AreEqual(6, BatteryCell.GetCellIDFromUpdateString(updateString));
            updateString = "<BC;7,234.3>";
            Assert.AreEqual(7, BatteryCell.GetCellIDFromUpdateString(updateString));
        }

        [Test]
        public void GetCellIDFromUpdateString_InvalidUpdateString_ExceptionThrown()
        {
            string updateString = "<BC;one,234.3>";
            Assert.Throws<InvalidUpdateStringException>(()=> BatteryCell.GetCellIDFromUpdateString(updateString));           
        }

        [Test]
        public void UpdateFromString_InvalidUpdateString_ThrowException()
        {
            BatteryCell bc = new BatteryCell(2);
            string updateString;

            updateString = "<>";
            Assert.Throws<InvalidUpdateStringException>(() => bc.UpdateFromString(updateString));
            updateString = "<B;23,45.2>";
            Assert.Throws<InvalidUpdateStringException>(() => bc.UpdateFromString(updateString));
            updateString = "<B,1,4>";
            Assert.Throws<InvalidUpdateStringException>(() => bc.UpdateFromString(updateString));
            updateString = "<B;3,45.1235>";
            Assert.Throws<InvalidUpdateStringException>(() => bc.UpdateFromString(updateString));
            updateString = "<B;,12>";
            Assert.Throws<InvalidUpdateStringException>(() => bc.UpdateFromString(updateString));
        }

        [Test]
        public void UpdateFromString_InvalidUpdateString_ObjectUpdated()
        {
            BatteryCell bc = new BatteryCell(4) { Voltage = 3.5f };
            string updateString;

            updateString = "<BC;4,3.6>";
            bc.UpdateFromString(updateString);

            Assert.AreEqual(3.6f, bc.Voltage);
        }

        [Test]
        public void GetUpdateString_ValidUpdateStringGenerated_ValidUpdateStringReturned()
        {
            BatteryCell bc = new BatteryCell(3) { Voltage = 3.2f };

            Assert.AreEqual(String.Format("<BC;{0},{1}>", bc.CellID, bc.Voltage), bc.GetUpdateString());
        }
    }
}
