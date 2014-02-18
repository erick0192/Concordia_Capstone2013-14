using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using MarsRover;

namespace MarsRoverTest
{
    class WatchDogTest
    {
        MarsRover.WatchDogCore wd;

        [Test]
        public void allowCommand_NotTimedOut_ReturnTrue()
        {
            wd = new MarsRover.WatchDogCore();
            wd.reportActivity();

            Assert.AreEqual(wd.allowCommand(), true);
        }

        [Test]
        public void reportActivity_NotTimedOut_AllowCommandAlwaysTrue()
        {
            int delay = 500;
            wd = new MarsRover.WatchDogCore();

            int i;
            for (i = 0; i < 10000; i += delay)
            {
                Thread.Sleep(delay);
                wd.reportActivity();

                Assert.AreEqual(wd.allowCommand(), true);
            }
        }

        [Test]
        public void allowCommand_TimedOut_ReturnFalse()
        {
            wd = new MarsRover.WatchDogCore();
            Thread.Sleep(5000);

            Assert.AreEqual(wd.allowCommand(), false);
        }

        [Test]
        public void reportActivity_TimedOut_AllowComandBecomesTrue()
        {
            wd = new MarsRover.WatchDogCore();
            Thread.Sleep(5000);

            bool before = wd.allowCommand();
            wd.reportActivity();
            bool after = wd.allowCommand();

            Assert.AreNotEqual(before, after);
            Assert.AreEqual(after, true);
        }
    }
}
