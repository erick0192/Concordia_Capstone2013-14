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
using MarsRover.Communication;

namespace RoverOperator
{
    public class StatusUpdater
    {
        #region Private fields
        
        private RoverStatus roverStatus;//We might not need this class anymore, as component updates are sent separately
        private volatile bool update;
        
        private volatile System.Timers.Timer timer;
        private IQueue updatesQueue;
        private MessageListener listener;

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

        public delegate void MotorsUpdatedDelegate(Dictionary<Motor.Location, Motor> updateString);
        public event  MotorsUpdatedDelegate MotorsUpdated;        

        public delegate void BatteryUpdatedDelegate(Battery battery);
        public event BatteryUpdatedDelegate BatteryUpdated;        

        public delegate void GPSCoordinatesUpdatedDelegate (GPSCoordinates gpsCoordinates);
        public event GPSCoordinatesUpdatedDelegate GPSCoordinatesUpdated;        


        #endregion

        private StatusUpdater()
        {
            roverStatus = new RoverStatus();
            //timer = new System.Timers.Timer(Properties.Settings.Default.StatusUpdateInterval) { Enabled = false };
            updatesQueue = new PriorityQueue(100);
            listener = new MessageListener(Properties.NetworkSettings.Default.StatusUpdatePort, updatesQueue, NetworkSettings.Instance.RoverIPAddress);
        }       

        #region Update Methods

        public void StartUpdating()
        {
            if (!IsUpdating)
            {
                update = true;
                //timer.Elapsed += new ElapsedEventHandler(this.Update);
                //timer.Enabled = true;
                listener.StartListening();
                
                Thread thread = new Thread(new ThreadStart(this.Update));
                thread.Start();
            }
        }

        public void StopUpdating()
        {            
            //update = false;
            //timer.Enabled = false;
            //timer.Elapsed -= new ElapsedEventHandler(this.Update);
        }

        private void Update()
        {
            int sleepPeriod = 100;           
            string updateData = "";

            while(update)
            {
                if(updatesQueue.TryDequeue(out updateData))
                {
                    UpdateGPS("");
                    UpdateBattery("");
                    UpdateMotors("");    
                }
                else
                {
                    Thread.Sleep(sleepPeriod); //Sleep for a bit and wait for data to fill up
                }
                
            }
            
        }

        private void UpdateMotors(String updateString)
        {
            //Parse all data from the other class that gets the data from the server
            Motor motor = roverStatus.Motors[Motor.Location.FrontLeft];
            motor.Current += 10;
            motor.Temperature += 1;

            motor = roverStatus.Motors[Motor.Location.FrontRight];
            motor.Current += 5;
            motor.Temperature += 4;

            motor = roverStatus.Motors[Motor.Location.MiddleLeft];
            motor.Current += 7;
            motor.Temperature += 3;

            motor = roverStatus.Motors[Motor.Location.MiddleRight];
            motor.Current += 7;
            motor.Temperature += 3;

            motor = roverStatus.Motors[Motor.Location.BackLeft];
            motor.Current += 7;
            motor.Temperature += 3;

            motor = roverStatus.Motors[Motor.Location.BackRight];
            motor.Current += 8;
            motor.Temperature += 2;

            if(MotorsUpdated != null)
            {
                MotorsUpdated(roverStatus.Motors);
            }
        }

        private void UpdateBattery(String updateString)
        {
            roverStatus.Battery.CurrentCharge -= 10;
            roverStatus.Battery.Temperature += 1;

            roverStatus.Battery.Cells.ForEach(cell => {
                cell.Voltage += 0.1f;
            });

            if(BatteryUpdated != null)
            {
                BatteryUpdated(roverStatus.Battery);
            }
        }

        private void UpdateGPS(String updateString)
        {
            if(GPSCoordinatesUpdated != null)
            {
                GPSCoordinatesUpdated(roverStatus.GPSCoordinates);
            }
        }

        #endregion
    }
}
