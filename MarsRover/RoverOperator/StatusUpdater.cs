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
    public class StatusUpdater
    {
        #region Private fields
        
        private RoverStatus roverStatus;
        private volatile bool update;
        
        private volatile System.Timers.Timer timer;
        private IQueue updatesQueue;
        private MessageListener listener;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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

        public delegate void MotorsUpdatedDelegate(Motor motor);
        public event  MotorsUpdatedDelegate MotorsUpdated;        

        public delegate void BatteryUpdatedDelegate(Battery battery);
        public event BatteryUpdatedDelegate BatteryUpdated;        

        public delegate void GPSCoordinatesUpdatedDelegate (GPSCoordinates gpsCoordinates);
        public event GPSCoordinatesUpdatedDelegate GPSCoordinatesUpdated;

        public delegate void BatteryCellUpdatedDelegate(BatteryCell batteryCell);
        public event BatteryCellUpdatedDelegate BatteryCellUpdated;  


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
            int sleepPeriod = 200;//Half a second           
            string updateData = string.Empty;

            while(update)
            {
                if(updatesQueue.TryDequeue(out updateData))
                {
                    try
                    {
                        string updateIdentifer = AbstractUpdateableComponent.GetUpdateIdentifierFromUpdateString(updateData);
                    
                        if(updateIdentifer == CommandMetadata.Update.MotorIdentifier)
                        {
                            UpdateMotors(updateData);        
                        }
                        else if (updateIdentifer == CommandMetadata.Update.BatteryIdentifier)
                        {
                            UpdateBattery(updateData);    
                        }
                        else if (updateIdentifer == CommandMetadata.Update.BatteryCellIdentifier)
                        {
                            UpdateBatteryCell(updateData);
                        }
                        else if (updateIdentifer == CommandMetadata.Update.GPSIdentfier)
                        {
                            UpdateGPS(updateData);
                        }
                        else
                        {                       
                            logger.Error("The update string '{0}' does not contain a valid update identifier.", updateData);
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

        private void UpdateMotors(String updateString)
        {
            //Parse all data from the other class that gets the data from the server
            //Motor motor = roverStatus.Motors[Motor.Location.FrontLeft];
            //motor.Current += 10;
            //motor.Temperature += 1;

            //motor = roverStatus.Motors[Motor.Location.FrontRight];
            //motor.Current += 5;
            //motor.Temperature += 4;

            //motor = roverStatus.Motors[Motor.Location.MiddleLeft];
            //motor.Current += 7;
            //motor.Temperature += 3;

            //motor = roverStatus.Motors[Motor.Location.MiddleRight];
            //motor.Current += 7;
            //motor.Temperature += 3;

            //motor = roverStatus.Motors[Motor.Location.BackLeft];
            //motor.Current += 7;
            //motor.Temperature += 3;

            //motor = roverStatus.Motors[Motor.Location.BackRight];
            //motor.Current += 8;
            //motor.Temperature += 2;     

            
                Motor m = roverStatus.Motors[Motor.GetLocationFromUpdateString(updateString)];
                m.UpdateFromString(updateString);

                if (MotorsUpdated != null)
                {
                    MotorsUpdated(m);
                }
            
               
        }

        private void UpdateBattery(string updateString)
        {
            //roverStatus.Battery.CurrentCharge -= 10;
            //roverStatus.Battery.Temperature += 1;

            //roverStatus.Battery.Cells.ForEach(cell => {
            //    cell.Voltage += 0.1f;
            //});

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
