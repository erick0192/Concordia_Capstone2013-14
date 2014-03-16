using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using MarsRover;
using MarsRover.Commands;
using MarsRover.Exceptions;

namespace RoverOperator
{
    public sealed class StatusUpdater
    {
        #region Private fields
        
        private RoverStatus roverStatus;
        private volatile bool update;
        
        private volatile System.Timers.Timer timer;
        private IQueue updatesQueue;
        private MessageListener listener;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static object syncRoot = new Object();

        #endregion

        #region Properties

        private static volatile StatusUpdater instance;
        public static StatusUpdater Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new StatusUpdater();
                        }
                    }
                }

                return instance;
            }
        }

        public RoverStatus RoverStatus { get { return roverStatus; } set { roverStatus = value; } }
        public bool IsUpdating { get { return update; } }

        #endregion

        #region Delegates and Events

        public delegate void MotorsUpdatedDelegate(Motor motor);
        public event  MotorsUpdatedDelegate MotorsUpdated;        

        public delegate void BatteryUpdatedDelegate(Battery battery);
        public event BatteryUpdatedDelegate BatteryUpdated;        

        public delegate void GPSCoordinatesUpdatedDelegate (GPSCoordinates gpsCoordinates);
        public event GPSCoordinatesUpdatedDelegate GPSCoordinatesUpdated;

        public delegate void BatteryCellUpdatedDelegate(BatteryCell batteryCell);
        public event BatteryCellUpdatedDelegate BatteryCellUpdated;

        public delegate void IMUUpdatedDelegate(IMU imu);
        public event IMUUpdatedDelegate IMUUpdated;

        public delegate void RoboticArmUpdatedDelegate(RoboticArm ra);
        public event RoboticArmUpdatedDelegate RoboticArmUpdated;

        #endregion

        private StatusUpdater()
        {
            roverStatus = new RoverStatus();
            //timer = new System.Timers.Timer(Properties.Settings.Default.StatusUpdateInterval) { Enabled = false };
            updatesQueue = new PriorityQueue(100);
            listener = new MessageListener(Properties.NetworkSettings.Default.StatusUpdatePort, updatesQueue, Properties.NetworkSettings.Default.RoverIPAddress);
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
            int sleepPeriod = 200;//Half a second           
            string updateString = string.Empty;

            while(update)
            {
                if(updatesQueue.TryDequeue(out updateString))
                {
                    try
                    {
                        string updateIdentifer = AbstractUpdateableComponent.GetUpdateIdentifierFromUpdateString(updateString);                        
                    
                        if(updateIdentifer == CommandMetadata.Update.MotorIdentifier)
                        {
                            UpdateMotors(updateString);        
                        }
                        else if (updateIdentifer == CommandMetadata.Update.BatteryIdentifier)
                        {
                            UpdateBattery(updateString);    
                        }
                        else if (updateIdentifer == CommandMetadata.Update.BatteryCellIdentifier)
                        {
                            UpdateBatteryCell(updateString);
                        }
                        else if (updateIdentifer == CommandMetadata.Update.GPSIdentfier)
                        {
                            UpdateGPS(updateString);
                        }
                        else if(updateIdentifer == CommandMetadata.Update.IMUIdentfier)
                        {
                            UpdateIMU(updateString);
                        }
                        else if(updateIdentifer == CommandMetadata.Update.RoboticArmIdentifier)
                        {
                            
                        }
                        else
                        {                       
                            logger.Error("The update string '{0}' does not contain a valid update identifier.", updateString);
                        }    
                    }
                    catch (Exception e)
                    {
                        logger.Error(e.Message);
                    }
                }
                else
                {
                    Thread.Sleep(sleepPeriod); //Sleep for a bit and wait for data to fill up
                }                
            }
            
        }

        private void UpdateRoboticArm(string updateString)
        {
            var ra = roverStatus.RoboArm;
            ra.UpdateFromString(updateString);

            if(RoboticArmUpdated != null)
            {
                RoboticArmUpdated(ra);
            }
        }

        private void UpdateIMU(string updateString)
        {
            var imu = roverStatus.IMUSensor;
            imu.UpdateFromString(updateString);

            if(IMUUpdated != null)
            {
                IMUUpdated(imu);
            }
        }

        private void UpdateMotors(String updateString)
        {
            Motor m = roverStatus.Motors[Motor.GetLocationFromUpdateString(updateString)];
            m.UpdateFromString(updateString);

            if (MotorsUpdated != null)
            {
                MotorsUpdated(m);
            }
        }

        private void UpdateBattery(string updateString)
        {
            roverStatus.Battery.UpdateFromString(updateString);

            if(BatteryUpdated != null)
            {
                BatteryUpdated(roverStatus.Battery);
            }
        }

        private void UpdateBatteryCell(string updateString)
        {
            var bc = roverStatus.Battery.Cells.First(c => c.CellID == BatteryCell.GetCellIDFromUpdateString(updateString));
            bc.UpdateFromString(updateString);
        }

        private void UpdateGPS(string updateString)
        {
            roverStatus.GPSCoordinates.UpdateFromString(updateString);

            if(GPSCoordinatesUpdated != null)
            {
                GPSCoordinatesUpdated(roverStatus.GPSCoordinates);
            }
        }

        #endregion
    }
}
