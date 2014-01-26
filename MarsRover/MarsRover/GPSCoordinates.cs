using MarsRover.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarsRover
{
    public class GPSCoordinates : IUpdateable
    {
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

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }       

        public bool IsMatch(string input)
        {
            return Regex.IsMatch(input, RegEx);
        }

        public void UpdateFromString(string updateString)
        {
            if (IsMatch(updateString))
            {
                var updateArray = updateString.Substring(1).Split(',');
                this.X = float.Parse(updateArray[0]);
                this.Y = float.Parse(updateArray[1]);
                this.Y = float.Parse(updateArray[2]);
            }
            else
            {
                throw new InvalidUpdateStringException(updateString);
            }
        }

        public string GetUpdateString()
        {
            return String.Format("G;{0},{1},{2}", X, Y, Z);
        }
    }
}
