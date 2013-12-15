using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotSoftware
{
    static class CommandMetadata
    {
        public const int CommandIdentifierIndex = 1;

        public static class Movement
        {
            //Example of assumed movement command format: <MF255F255>

            public const char IdentifierCharacter = 'M';

            public const int LeftDirectionIndex = 2;
            public const int RightDirectionIndex = 6;

            public const int LeftSpeedStartIndex = 3;
            public const int RightSpeedStartIndex = 7;

            public const int LeftSpeedEndIndex = 5;
            public const int RightSpeedEndIndex = 9;
        }
    }
}
