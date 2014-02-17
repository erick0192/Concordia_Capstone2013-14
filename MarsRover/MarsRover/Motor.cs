using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover.Exceptions;
using System.Text.RegularExpressions;

namespace MarsRover
{
    public class Motor : AbstractUpdateableComponent
    {
        public enum Location
        {
            FrontLeft,
            FrontRight,
            MiddleLeft,
            MiddleRight,
            BackLeft,
            BackRight
        }

        public enum TemperatureStatus
        {
            Normal,
            Warning,
            Dangerous
        }

        public enum CurrentStatus
        {
            Normal,
            Warning,
            Dangerous
        }

        //Amperes
        public const float MIN_CURRENT = 0.0f;
        public const float MIN_WARNING_CURRENT = 5.0f;
        public const float MAX_CURRENT = 20.0f;
        public const float MAX_WARNING_CURRENT = 15.0f;

        //Celsius
        public const float MIN_TEMPERATURE = 0.0f;
        public const float MIN_WARNING_TEMPERATURE = 20.0f;
        public const float MAX_TEMPERATURE = 120.0f;
        public const float MAX_WARNING_TEMPERATURE = 100.0f;

        #region Properties

        private float current;
        public Motor.CurrentStatus StatusCurrent { get; set; }
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

        private float temperature;
        public Motor.TemperatureStatus StatusTemperature { get; set; }
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
        


        public float Duty { get; set; }
        public Motor.Location LocationOnRover { get; set; }

        public override string UpdateIdentifier
        {
            get { return MarsRover.Commands.CommandMetadata.Update.MotorIdentifier; }
        }

        #endregion

        #region Delegates and Events

        public delegate void WarningCurrentDetectedDelegate(Motor motor);
        public event WarningCurrentDetectedDelegate WarningCurrentDetected;

        public delegate void DangerousCurrentDetectedDelegate(Motor motor);
        public event WarningCurrentDetectedDelegate DangerousCurrentDetected;

        public delegate void NormalCurrentDetectedDelegate(Motor motor);
        public event NormalCurrentDetectedDelegate NormalCurrentDetected;

        public delegate void WarningTemperatureDetectedDelegate(Motor motor);
        public event WarningTemperatureDetectedDelegate WarningTemperatureDetected;

        public delegate void DangerousTemperatureDetectedDelegate(Motor motor);
        public event DangerousTemperatureDetectedDelegate DangerousTemperatureDetected;

        public delegate void NormalTemperatureDetectedDelegate(Motor motor);
        public event NormalTemperatureDetectedDelegate NormalTemperatureDetected;

        //public delegate void 

        #endregion

        #region Constructor

        public Motor(Motor.Location location)
        {
            LocationOnRover = location;
            regex = "<" + UpdateIdentifier + @";[MFB],[LR],\d+(\.\d{1,3})?,\d+(\.\d{1,3})?>";
        }

        #endregion

        #region Methods

        private void CheckCurrentStatus()
        {
            if ((current >= MAX_CURRENT || current <= MIN_CURRENT)
                && StatusCurrent != CurrentStatus.Dangerous)
            {
                StatusCurrent = CurrentStatus.Dangerous;
                if (DangerousCurrentDetected != null)
                {
                    DangerousCurrentDetected(this);
                }
            }
            else if ((current > MAX_WARNING_CURRENT || current < MIN_WARNING_CURRENT)
                && StatusCurrent != CurrentStatus.Warning)
            {
                StatusCurrent = CurrentStatus.Warning;
                if (WarningCurrentDetected != null)
                {
                    WarningCurrentDetected(this);
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
            if ((temperature >= MAX_TEMPERATURE || temperature <= MIN_TEMPERATURE)
                && StatusTemperature != TemperatureStatus.Dangerous)
            {
                StatusTemperature = TemperatureStatus.Dangerous;
                if(DangerousTemperatureDetected != null)
                {
                    DangerousTemperatureDetected(this);
                }
            }
            else if ((temperature > MAX_WARNING_TEMPERATURE || temperature < MIN_WARNING_TEMPERATURE)
                && StatusTemperature != TemperatureStatus.Warning)
            {
                StatusTemperature = TemperatureStatus.Dangerous;
                if(WarningTemperatureDetected!= null)
                {
                    WarningTemperatureDetected(this);
                }
            }
            else if(StatusTemperature != TemperatureStatus.Normal)
            {
                StatusTemperature = TemperatureStatus.Normal;
                if(NormalTemperatureDetected != null)
                {
                    NormalTemperatureDetected(this);
                }
            }
        }

        public static string GetLocationFriendlyString(Motor.Location location)
        {
            switch (location)
            {
                case Motor.Location.FrontLeft:
                    return "Front Left";
                case Motor.Location.FrontRight:
                    return "Front Right";
                case Motor.Location.MiddleLeft:
                    return "Middle Left";
                case Motor.Location.MiddleRight:
                    return "Middle Right";
                case Motor.Location.BackLeft:
                    return "Back Left";
                case Motor.Location.BackRight:
                    return "Back Right";
            }

            return "";
        }

        public static Motor.Location GetLocationFromUpdateString(string updateString)
        {
            var updateArray = GetUpdateStringArrayWithoutIdentifier(updateString);

            if(updateArray[0] == "F")
            {
                if(updateArray[1] == "L")
                {
                    return Motor.Location.FrontLeft;
                }
                else if(updateArray[1] == "R")
                {
                    return Motor.Location.FrontRight;
                }              
            }
            else if(updateArray[0] == "M")
            {
                if (updateArray[1] == "L")
                {
                    return Motor.Location.MiddleLeft;
                }
                else if (updateArray[1] == "R")
                {
                    return Motor.Location.MiddleRight;
                }               
            }
            else if(updateArray[0] == "B")
            {
                if (updateArray[1] == "L")
                {
                    return Motor.Location.BackLeft;
                }
                else if (updateArray[1] == "R")
                {
                    return Motor.Location.BackRight;
                }               
            }
            
            throw new InvalidUpdateStringException(updateString);
            
        }        

        public override void UpdateFromString(string updateString)
        {
            if (IsValidUpdateString(updateString))
            {
                if (Motor.GetLocationFromUpdateString(updateString) == LocationOnRover)
                {
                    // We dont want to include the identifier nor the last bracket
                    var updateArray = GetUpdateStringArrayWithoutIdentifier(updateString);
                    this.Current = float.Parse(updateArray[2]);
                    this.Temperature = float.Parse(updateArray[3]);
                }
                else
                {
                    throw new InvalidUpdateStringException(updateString, "The motor location does not match the one indicated  by the update string.");
                }
            }
            else
            {
                throw new InvalidUpdateStringException(updateString);
            }
        }

        public override string GetUpdateString()
        {
            string motorBodyLocation = string.Empty, motorSideLocation = string.Empty;

            switch (this.LocationOnRover)
            {
                case Motor.Location.FrontLeft:
                    motorBodyLocation = "F";
                    motorSideLocation = "L";
                    break;
                case Motor.Location.FrontRight:
                    motorBodyLocation = "F";
                    motorSideLocation = "R";
                    break;
                case Motor.Location.MiddleLeft:
                    motorBodyLocation = "M";
                    motorSideLocation = "L";
                    break;
                case Motor.Location.MiddleRight:
                    motorBodyLocation = "M";
                    motorSideLocation = "R";
                    break;
                case Motor.Location.BackLeft:
                    motorBodyLocation = "B";
                    motorSideLocation = "L";
                    break;
                case Motor.Location.BackRight:
                    motorBodyLocation = "B";
                    motorSideLocation = "R";
                    break;
            }

            return CreateUpdateString(
                motorBodyLocation, motorSideLocation, Math.Round(Current, 3), Math.Round(Temperature, 3)
                );
        }

        #endregion                    
    }
}
