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
    }
}
