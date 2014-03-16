using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Commands
{
    public static class CommandMetadata
    {
        public const int IdIndex = 1;
        public const int IdLength = 1; //Perhaps we will have to set the ID length to 2 in the future.
        public const string StartDelimiter = "<";
        public const string EndDelimiter = ">";

        public static class OldMovement
        {
            //Obselete. Old movement command format that was used. Kept in case of a required backup
            //Example: <MF255F255>

            public const string Identifier = "M";
            public const string Forward = "F";
            public const string Backward = "B";

            public const int MaxSpeed = 255;

            public const int DirectionLength = 1;
            public const int IdentifierLength = 1;

            public const int LeftDirectionIndex = 2;
            public const int RightDirectionIndex = 6;

            public const int LeftSpeedStartIndex = 3;
            public const int RightSpeedStartIndex = 7;

            public const int LeftSpeedEndIndex = 5;
            public const int RightSpeedEndIndex = 9;
        }

        public static class Movement
        {
            //Example: <LF255F255F255>
            public const string LeftIdentifier = "L";
            public const string RightIdentifier = "R";
            public const string Forward = "F";
            public const string Backward = "B";

            public const int MotorSideIndex = 1;

            public const int MaxSpeed = 255;
            public const int MinSpeed = 0;

            public const int DirectionLength = 1;
            public const int IdentifierLength = 1;

            public const int Motor1DirectionIndex = 2;
            public const int Motor2DirectionIndex = 6;
            public const int Motor3DirectionIndex = 10;

            public const int Motor1SpeedStartIndex = 3;
            public const int Motor2SpeedStartIndex = 7;
            public const int Motor3SpeedStartIndex = 11;

            public const int Motor1SpeedEndIndex = 5;
            public const int Motor2SpeedEndIndex = 9;
            public const int Motor3SpeedEndIndex = 13;
        }

        public static class Camera
        {
            //Example cam on/off command: <C1F>
            public const string Identifier = "C";
            public const string On = "O";
            public const string Off = "F";

            public const int NumberIndex = 2;
            public const int NumberLength = 1;
            public const int StatusIndex = 3;
            public const int StatusLength = 1;
        }

        public static class Pan
        {
            //Example command: <P0359>
            public const string Identifier = "P";
            public const int MaxPanAngle = 200;
            public const int MinPanAngle = 0;

            public const int NumberIdentifierIndex = 2;
            public const int NumberIdentifierLength = 1;

            public const int AngleStartIndex = 3;
            public const int AngleEndIndex = 5;
        }

        public static class Tilt
        {
            //Example command: <T1000>
            public const string Identifier = "T";
            public const int MaxTiltAngle = 90;
            public const int MinTiltAngle = 0;

            public const int NumberIdentifierIndex = 2;
            public const int NumberIdentifierLength = 1;

            public const int AngleStartIndex = 3;
            public const int AngleEndIndex = 5;
        }

        public static class Update
        {
            public const string BatteryIdentifier = "B";
            public const string BatteryCellIdentifier = "BC";
            public const string MotorIdentifier = "MR";
            public const string GPSIdentfier = "G";
            public const string IMUIdentfier = "I";
            public const string RoboticArmIdentifier = "RA";

            public const char ValuesDelimiter = ',';
            public const char StartOfValuesIdentifier = ';';
        }
    }
}