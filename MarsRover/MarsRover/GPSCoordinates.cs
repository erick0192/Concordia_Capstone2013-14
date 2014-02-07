using MarsRover.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarsRover.Commands;

namespace MarsRover
{
    public class GPSCoordinates : AbstractUpdateableComponent
    {
        #region Properties

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

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

        public override void UpdateFromString(string updateString)
        {
            if (IsValidUpdateString(updateString))
            {
                var updateArray = GetUpdateStringArrayWithoutIdentifier(updateString);
                this.X = float.Parse(updateArray[0], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                this.Y = float.Parse(updateArray[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                this.Z = float.Parse(updateArray[2], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
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
