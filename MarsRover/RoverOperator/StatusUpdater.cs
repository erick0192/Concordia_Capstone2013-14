/** 
 * This singleton class will be in charge of using the class responsible for requesting
 * data from the server and update each of the robots components, i.e., battery, motors, etc.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using MarsRover;

namespace RoverOperator
{
    public class StatusUpdater
    {
        #region Private fields
        
        private RoverStatus roverStatus;
        private volatile bool update;

        //temporary until we get communication going
        private volatile System.Timers.Timer timer;

        #endregion

        #region Properties

        private static StatusUpdater instance;
        public static StatusUpdater Instance
        {
            get
            {
                if (null == instance)
                    instance = new StatusUpdater();

                return instance;
            }
        }

        public RoverStatus RoverStatus { get { return roverStatus; } set { roverStatus = value; } }
        public bool IsUpdating { get { return update; } }

        #endregion

        #region Delegates and Events

        public delegate void RoverStatusUpdatedEventHandler(RoverStatus roverStatus);
        public event RoverStatusUpdatedEventHandler RoverStatusUpdated;        

        #endregion

        private StatusUpdater()
        {
            roverStatus = new RoverStatus();
            timer = new System.Timers.Timer(1000) { Enabled = false };
        }       

        #region Update Methods

        public void StartUpdating()
        {
            if (!IsUpdating)
            {
                update = true;
                timer.Enabled = true;
                Thread thread = new Thread(new ThreadStart(this.Update));
                thread.Start();
            }
        }

        public void StopUpdating()
        {            
            update = false;
        }

        private void Update()
        {
            //temporary until we get communication going
            timer.Elapsed += new ElapsedEventHandler(TemporaryHandler);

            while(update)
            {
                //Here we would listen for incoming data on a udp port
            }

            timer.Enabled = false;
            timer.Elapsed -= new ElapsedEventHandler(TemporaryHandler);
        }

        private void TemporaryHandler(object sender, ElapsedEventArgs e)
        {
            UpdateRoverStatus("");
        }

        private void UpdateRoverStatus(String statusString)
        {
            //Parse all data from the other class that gets the data from the server
            Motor motor = roverStatus.Motors[Motor.Location.FrontLeft];
            motor.Current += 10;
            motor.Temperature += 1;

            motor = roverStatus.Motors[Motor.Location.FrontRight];
            motor.Current += 5;
            motor.Temperature += 4;

            motor = roverStatus.Motors[Motor.Location.BackLeft];
            motor.Current += 7;
            motor.Temperature += 3;

            motor = roverStatus.Motors[Motor.Location.BackRight];
            motor.Current += 8;
            motor.Temperature += 2;

            roverStatus.Battery.CurrentCharge -= 10;
            roverStatus.Battery.Temperature += 1;

            if (RoverStatusUpdated != null)
                RoverStatusUpdated(roverStatus);
        }

        #endregion
    }
}
