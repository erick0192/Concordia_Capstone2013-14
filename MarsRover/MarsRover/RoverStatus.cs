using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover
{
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

    public delegate void WarningCurrentDetectedDelegate<T>(T roverComponent);
    public delegate void DangerousCurrentDetectedDelegate<T>(T roverComponent);
    public delegate void NormalCurrentDetectedDelegate<T>(T roverComponent);

    public delegate void WarningTemperatureDetectedDelegate<T>(T roverComponent);
    public delegate void DangerousTemperatureDetectedDelegate<T>(T roverComponent);
    public delegate void NormalTemperatureDetectedDelegate<T>(T roverComponent);

    public class RoverStatus
    {
        public Battery Battery { get; set; }
        public Dictionary<Motor.Location, Motor> Motors { get; set; }
        public GPSCoordinates GPSCoordinates { get; set; }
        public IMU IMUSensor { get; set; }
        public RoboticArm RoboArm { get; set; }

        public RoverStatus()
        {
            Motors = new Dictionary<Motor.Location, Motor>(6);
            Motors.Add(Motor.Location.FrontLeft, new Motor(Motor.Location.FrontLeft));
            Motors.Add(Motor.Location.FrontRight, new Motor(Motor.Location.FrontRight));
            Motors.Add(Motor.Location.BackLeft, new Motor(Motor.Location.BackLeft));
            Motors.Add(Motor.Location.BackRight, new Motor(Motor.Location.BackRight));
            Motors.Add(Motor.Location.MiddleLeft, new Motor(Motor.Location.MiddleLeft));
            Motors.Add(Motor.Location.MiddleRight, new Motor(Motor.Location.MiddleRight));

            Battery = new Battery(2000);
            GPSCoordinates = new GPSCoordinates();
            IMUSensor = new IMU();
            RoboArm = new RoboticArm();
        }
    }
}
