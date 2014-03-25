using MarsRover.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarsRover.Commands;
using Microsoft.Maps.MapControl.WPF;

namespace MarsRover
{
    public class GPSCoordinates : AbstractUpdateableComponent
    {
        #region Properties

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Location Location { get; set; }

        public override string UpdateIdentifier
        {
            get { return CommandMetadata.Update.GPSIdentfier; }
        }

        #endregion

        #region Constructor

        public GPSCoordinates()
        {
            regex = "<" + UpdateIdentifier + @";[-]?\d+(\.\d{1,6})?,[-]?\d+(\.\d{1,6})?,[-]?\d+(\.\d{1,6})?>";
            X = 90.0;
            Y = 90.0;
            Z = 90.0;
        }

        #endregion

        #region Methods
        //NMEA in X, Y, Z format methods for longitude and latitude
        private double nmeaToLatitude(double Z)
        {
            return Math.Asin(Z / 6371) * 57.2957795; 
            //6371 is approximate radius of the earth in km, 57.2957795 degrees per radian
        }
        private double nmeaToLongitude(double X, double Y)
        {
            return Math.Atan2(Y, X) * 57.2957795; 
            //57.2957795 degrees per radian
        }

        #endregion

        public override void UpdateFromString(string updateString)
        {
            if (IsValidUpdateString(updateString))
            {
                var updateArray = GetUpdateStringArrayWithoutIdentifier(updateString);
                this.X = float.Parse(updateArray[0], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                this.Y = float.Parse(updateArray[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                this.Z = float.Parse(updateArray[2], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);

                if (this.Location == null)
                {
                    Location = new Location();
                }

                this.Location.Latitude = nmeaToLatitude(this.Z);
                this.Location.Longitude = nmeaToLongitude(this.X, this.Y);
            }
            else
            {
                throw new InvalidUpdateStringException(updateString);
            }
        }

        public override string GetUpdateString()
        {
            return CreateUpdateString(
                Math.Round(X, 6),
                Math.Round(Y, 6),
                Math.Round(Z, 6));
        }
       
    }
}
