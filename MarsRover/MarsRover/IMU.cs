using MarsRover.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;

namespace MarsRover
{
    public class IMU : AbstractUpdateableComponent
    {
        #region Properties

        //Degrees
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public override string UpdateIdentifier
        {
            get { return MarsRover.Commands.CommandMetadata.Update.IMUIdentfier; }
        }

        #endregion

        #region Constructor

        public IMU()
        {
            regex = "<" + UpdateIdentifier + @";[-]?\d+(\.\d{1,6})?,[-]?\d+(\.\d{1,6})?,[-]?\d+(\.\d{1,6})?>";
        }

        #endregion

        #region Methods

        public override void UpdateFromString(string updateString)
        {
            if (IsValidUpdateString(updateString))
            {
                var updateArray = GetUpdateStringArrayWithoutIdentifier(updateString);
                this.Yaw = float.Parse(updateArray[0],NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                this.Pitch = float.Parse(updateArray[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                this.Roll = float.Parse(updateArray[2], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
            }
            else
            {
                throw new InvalidUpdateStringException(updateString);
            }
        }

        public override string GetUpdateString()
        {
            return CreateUpdateString(
               Math.Round(this.Yaw, 6),
               Math.Round(this.Pitch, 6),
               Math.Round(this.Roll, 6));
        }

        #endregion
    }
}
