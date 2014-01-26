using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rover;

namespace MarsRoverTest
{
    [TestFixture, Category("GPSLogTests")]
    class GPSLogTests
    {
        [Test]
        public void CreateGPSSensor_ValidCommandReceived_InitializesProperly()
        {
            string rawData = "G45.462257,-73.573379,32.50";
            string latitudeExpected = "45.462257";
            string longitudeExpected = "-73.573379";
            string altitudeExpected = "32.50";

            GPSLog gps = new GPSLog(rawData);
            Assert.AreEqual(latitudeExpected, gps.Latitude);
            Assert.AreEqual(longitudeExpected, gps.Longitude);
            Assert.AreEqual(altitudeExpected, gps.Altitude);
            Assert.IsTrue(gps.IsUpdated);
        }

        [Test]
        public void CreateGPSSensor_ArgumentMissing_ThrowsArgumentException()
        {
            string rawData = "G45.462257,-73.573379";
            Assert.Throws<System.ArgumentException>(
                delegate
                {
                    GPSLog gps = new GPSLog(rawData);
                }
            , "Command sent: G45.462257,-73.573379");

        }

        [Test]
        public void LogData_InitializedProperly_IsUpdatedIsFalse()
        {
        }

        [Test]
        public void UpdateData_ValidInputProvided_IsUpdatedIsTrue()
        {
        }
    }
}
