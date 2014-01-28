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
    public class Motor : IUpdateable
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

        //Amperes
        public const float MIN_CURRENT = 0.0f;
        public const float MAX_CURRENT = 20.0f;

        //Celsius
        public const float MIN_TEMPERATURE = 0.0f;
        public const float MAX_TEMPERATYRE = 120.0f;

        #region Properties

        public float Current { get; set; }
        public float Temperature { get; set; }
        public Motor.Location LocationOnRover { get; set; }

        private string regex;
        public string RegEx
        {
            get
            {
                return regex;
            }
            set
            {
                regex = value;
            }
        }

        #endregion

        #region Constructor

        public Motor(Motor.Location location)
        {
            LocationOnRover = location;
            regex = @"/<MR;[MFB],[LR],\d+(\.\d{1,2})?,\d+(\.\d{1,2})?>/";
        }

        #endregion

        #region Methods

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
            var updateArray = updateString.Substring(updateString.IndexOf(";")).Split(',');

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

        public bool IsMatch(string input)
        {
            return Regex.IsMatch(input, RegEx);
        }

        public void UpdateFromString(string updateString)
        {
            if(IsMatch(updateString))
            {
                var updateArray = updateString.Substring(updateString.IndexOf(";")).Split(',');
                this.Current = float.Parse(updateArray[2]);
                this.Temperature = float.Parse(updateArray[3]);
            }
            else
            {
                throw new InvalidUpdateStringException(updateString);
            }
        }

        public string GetUpdateString()
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

            return String.Format("MR;{0},{1},{2},{3}", 
                motorBodyLocation, motorSideLocation, Current, Temperature);
        }

        #endregion                    
    }
}
