using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover
{
    public class Motor
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

        public int Current { get; set; }
        public int Temperature { get; set; }
        public Motor.Location LocationOnRover { get; set; }

        #endregion

        #region Constructor

        public Motor(Motor.Location location)
        {
            LocationOnRover = location;
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

        #endregion
    }
}
