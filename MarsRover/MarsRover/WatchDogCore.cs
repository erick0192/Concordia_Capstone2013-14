using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public class WatchDogCore
    {
        #region Attributes

        private const double BASE_TIME = 2.5f;

        private Stopwatch _stopwatch;

        #endregion

        #region Constructors

        public WatchDogCore()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        #endregion

        #region Methods

        public void reportActivity()
        {
            _stopwatch.Restart();
        }

        public bool allowCommand()
        {
            return (BASE_TIME > _stopwatch.ElapsedMilliseconds / 1000.0);
        }

        #endregion
    }
}
