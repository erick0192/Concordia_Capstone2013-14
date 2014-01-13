using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover
{
    public class RoverStatus
    {
        public Battery Battery { get; set; }
        public Dictionary<Motor.Location, Motor> Motors { get; set; }
        public GPSCoordinates GPSCoordinates { get; set; }

        public RoverStatus()
        {
            Motors = new Dictionary<Motor.Location, Motor>(6);
            Motors.Add(Motor.Location.FrontLeft, new Motor());
            Motors.Add(Motor.Location.FrontRight, new Motor());            
            Motors.Add(Motor.Location.BackLeft, new Motor());
            Motors.Add(Motor.Location.BackRight, new Motor());
            Motors.Add(Motor.Location.MiddleLeft, new Motor());
            Motors.Add(Motor.Location.MiddleRight, new Motor());

            Battery = new Battery(2000);
            GPSCoordinates = new GPSCoordinates();
        }
    }
}
