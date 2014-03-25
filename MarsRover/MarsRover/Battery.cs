using MarsRover.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarsRover
{
    public class Battery : AbstractUpdateableComponent
    {
        //Amperes
        public const float MIN_CURRENT = 0.0F;
        public const float MIN_WARNING_CURRENT = 50.0f;
        public const float MAX_CURRENT = 300.0f;
        public const float MAX_WARNING_CURRENT = 250.0f;

        //Celsius
        public const float MIN_TEMPERATURE = 0.0f;
        public const float MIN_WARNING_TEMPERATURE = 20.0f;
        public const float MAX_TEMPERATURE = 120.0f;
        public const float MAX_WARNING_TEMPERATURE = 100.0f;

        #region Properties

        public int CurrentCharge { get; set; }       
        public int MaxCharge { get; set; }

        private float temperature;
        public TemperatureStatus StatusTemperature { get; set; }
        public float Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                temperature = value;
                CheckTemperatureStatus();
            }
        }

        public float ChargePerc { get; set; }
        
        private float current;
        public CurrentStatus StatusCurrent { get; set; }
        public float Current
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
                CheckCurrentStatus();
            }
        }

        public override string UpdateIdentifier
        {
            get { return MarsRover.Commands.CommandMetadata.Update.BatteryIdentifier; }
        }

        public float ChargeRatio
        {
            get { return (float)Math.Round(((float)CurrentCharge / MaxCharge), 2); }
        }

        public float ChargePercentage
        {
            get { return (float)Math.Round(((float)CurrentCharge / MaxCharge) * 100, 2); }
        }

        private List<BatteryCell> cells = new List<BatteryCell>(7);
        public List<BatteryCell> Cells { get { return cells; } }

        #endregion

        #region Delegate and Events

        public event WarningCurrentDetectedDelegate<Battery> WarningCurrentDetected;
        public event DangerousCurrentDetectedDelegate<Battery> DangerousCurrentDetected;
        public event NormalCurrentDetectedDelegate<Battery> NormalCurrentDetected;

        public event WarningTemperatureDetectedDelegate<Battery> WarningTemperatureDetected;
        public event DangerousTemperatureDetectedDelegate<Battery> DangerousTemperatureDetected;
        public event NormalTemperatureDetectedDelegate<Battery> NormalTemperatureDetected;

        #endregion

        #region Constructor

        public Battery(int maxCharge)
        {
            MaxCharge = maxCharge;
            CurrentCharge = maxCharge;
            regex = "<" + UpdateIdentifier + @";\d+(\.\d{1,3})?,\d+(\.\d{1,3})?,\d+(\.\d{1,3})?>";

            for(int i = 0; i < cells.Capacity; i++)
            {
                cells.Add(new BatteryCell(i + 1) { Voltage = 0});                
            }
        }

        #endregion

        #region Methods

        private void CheckCurrentStatus()
        {
            if (current >= MAX_CURRENT || current <= MIN_CURRENT)
            {
                if (StatusCurrent != CurrentStatus.Dangerous)
                {
                    StatusCurrent = CurrentStatus.Dangerous;
                    if (DangerousCurrentDetected != null)
                    {
                        DangerousCurrentDetected(this);
                    }
                }
            }
            else if (current > MAX_WARNING_CURRENT || current < MIN_WARNING_CURRENT)
            {
                if (StatusCurrent != CurrentStatus.Warning)
                {
                    StatusCurrent = CurrentStatus.Warning;
                    if (WarningCurrentDetected != null)
                    {
                        WarningCurrentDetected(this);
                    }
                }
            }
            else if (StatusCurrent != CurrentStatus.Normal)
            {
                StatusCurrent = CurrentStatus.Normal;
                if (NormalCurrentDetected != null)
                {
                    NormalCurrentDetected(this);
                }
            }
        }

        private void CheckTemperatureStatus()
        {
            if (temperature >= MAX_TEMPERATURE || temperature <= MIN_TEMPERATURE)
            {
                if (StatusTemperature != TemperatureStatus.Dangerous)
                {
                    StatusTemperature = TemperatureStatus.Dangerous;
                    if (DangerousTemperatureDetected != null)
                    {
                        DangerousTemperatureDetected(this);
                    }
                }
            }
            else if (temperature > MAX_WARNING_TEMPERATURE || temperature < MIN_WARNING_TEMPERATURE)
            {
                if (StatusTemperature != TemperatureStatus.Warning)
                {
                    StatusTemperature = TemperatureStatus.Warning;
                    if (WarningTemperatureDetected != null)
                    {
                        WarningTemperatureDetected(this);
                    }
                }
            }
            else if (StatusTemperature != TemperatureStatus.Normal)
            {
                StatusTemperature = TemperatureStatus.Normal;
                if (NormalTemperatureDetected != null)
                {
                    NormalTemperatureDetected(this);
                }
            }
        }

        public override void UpdateFromString(string updateString)
        {
            if (IsValidUpdateString(updateString))
            {
                var updateArray = GetUpdateStringArrayWithoutIdentifier(updateString);

                this.ChargePerc = float.Parse(updateArray[0]);
                this.Current = float.Parse(updateArray[1]);
                this.Temperature = float.Parse(updateArray[2]);
            }
            else
            {
                throw new InvalidUpdateStringException(updateString);
            }
        }

        public override string GetUpdateString()
        {
            return CreateUpdateString(Math.Round(ChargePerc, 3), Math.Round(Current, 3), Math.Round(Temperature, 3));
        }

        #endregion
    }
}
