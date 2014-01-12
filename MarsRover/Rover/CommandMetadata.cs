using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rover
{
    static class CommandMetadata
    {
        public const int IdIndex = 1;
        public const int IdLength = 1; //Perhaps we will have to set the ID length to 2 in the future.

        public static class Movement
        {
            //Example of assumed movement command format: <MF255F255>

            public const string Identifier = "M";

            public const int LeftDirectionIndex = 2;
            public const int RightDirectionIndex = 6;

            public const int LeftSpeedStartIndex = 3;
            public const int RightSpeedStartIndex = 7;

            public const int LeftSpeedEndIndex = 5;
            public const int RightSpeedEndIndex = 9;
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
    }
}
