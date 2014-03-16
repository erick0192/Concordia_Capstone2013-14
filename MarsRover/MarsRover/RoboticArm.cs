using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover.Exceptions;

namespace MarsRover
{
    public class RoboticArm : AbstractUpdateableComponent
    {

        public RoboticArm()
        {
            regex =
                Commands.CommandMetadata.StartDelimiter +
                UpdateIdentifier + Commands.CommandMetadata.Update.StartOfValuesIdentifier +
                "" +
                Commands.CommandMetadata.EndDelimiter;
        }

        #region Methods

        public override string UpdateIdentifier
        {
            get { return Commands.CommandMetadata.Update.RoboticArmIdentifier; }
        }

        public override void UpdateFromString(string updateString)
        {
            if(IsValidUpdateString(updateString))
            {
                
            }
            else
            {
                throw new InvalidUpdateStringException(updateString);
            }
        }

        public override string GetUpdateString()
        {
            return string.Empty;
        }

        #endregion
    }
}
