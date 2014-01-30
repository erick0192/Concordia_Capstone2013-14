using MarsRover.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarsRover
{
    public class GPSCoordinates : AbstractUpdateable
    {

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override void UpdateFromString(string updateString)
        {
            if (IsValidUpdateString(updateString))
            {
                var updateArray = GetUpdateStringArrayWithoutIdentifier(updateString);
                this.X = float.Parse(updateArray[0]);
                this.Y = float.Parse(updateArray[1]);
                this.Y = float.Parse(updateArray[2]);
            }
            else
            {
                throw new InvalidUpdateStringException(updateString);
            }
        }

        public override string GetUpdateString()
        {
            return String.Format("G;{0},{1},{2}", X, Y, Z);
        }
       
    }
}
