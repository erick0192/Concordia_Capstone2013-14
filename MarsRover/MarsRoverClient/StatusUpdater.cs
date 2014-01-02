/** 
 * This singleton class will be in charge of using the class responsible for requesting
 * data from the server and update each of the robots components, i.e., battery, motors, etc.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MarsRover;

namespace MarsRoverClient
{
    public class StatusUpdater
    {
        #region Private fields

        private Dictionary<String, Motor> motors;
        private Battery battery;

        private Timer updateMotorsTimer;
        private Timer updateBatteryTimer;

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

        public int MotorsUpdateInterval
        {
            get
            {
                return Properties.Settings.Default.MotorsUpdateInterval;
            }
            set
            {
                Properties.Settings.Default.MotorsUpdateInterval = value;
                updateMotorsTimer.Interval = value;
            }
        }

        public int BatteryUpdateInterval
        {
            get
            {
                return Properties.Settings.Default.BatteryUpdateInterval;
            }
            set
            {
                Properties.Settings.Default.BatteryUpdateInterval = value;
                updateBatteryTimer.Interval = value;
            }
        }

        public Battery Battery { get { return battery; } }

        #endregion

        #region Delegates and Events

        public delegate void MotorsStatusUpdatedEventHandler(Dictionary<String,Motor> motors);
        public event MotorsStatusUpdatedEventHandler MotorsStatusUpdated;

        public delegate void BatteryStatusUpdatedEventHandler(Battery battery);
        public event BatteryStatusUpdatedEventHandler BatteryStatusUpdated;

        #endregion

        private StatusUpdater()
        {
            motors = new Dictionary<string, Motor>(4);
            motors.Add("FrontLeft", new Motor());
            motors.Add("FrontRight", new Motor());
            motors.Add("BackLeft", new Motor());
            motors.Add("BackRight", new Motor());

            battery = new Battery(2000);

            updateMotorsTimer = new Timer(MotorsUpdateInterval) { Enabled = false };
            updateMotorsTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateMotors);
            
            
            updateBatteryTimer = new Timer(BatteryUpdateInterval) {Enabled = false };
            updateBatteryTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateBattery);
        }

        public void StartUpdating()
        {
            updateBatteryTimer.Enabled = true;
            updateMotorsTimer.Enabled = true;
        }

        public void StopUpdating()
        {
            updateBatteryTimer.Enabled = false;
            updateMotorsTimer.Enabled = false;
        }

        #region Update Methods

        private void UpdateMotors(object source, ElapsedEventArgs e)
        {
            //Parse all data from the other class that gets the data from the server
            Motor motor = motors["FrontLeft"];
            motor.Current += 10;
            motor.Temperature += 1;

            motor = motors["FrontRight"];
            motor.Current += 5;
            motor.Temperature += 4;

            motor = motors["BackLeft"];
            motor.Current += 7;
            motor.Temperature += 3;

            motor = motors["BackRight"];
            motor.Current += 8;
            motor.Temperature += 2;

            if (MotorsStatusUpdated != null)
                MotorsStatusUpdated(motors);
        }

        private void UpdateBattery(object source, ElapsedEventArgs e)
        {
            battery.CurrentCharge -= 10;
            battery.Temperature += 1;

            if (BatteryStatusUpdated != null)
                BatteryStatusUpdated(battery);
        }


        #endregion
    }
}
