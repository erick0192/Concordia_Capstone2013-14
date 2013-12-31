using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover
{
    public class Battery
    {
        #region Properties

        public int CurrentCharge { get; set; }
        public int MaxCharge { get; set; }
        public int Temperature { get; set; }

        public float ChargePercentage
        {
            get { return (float)Math.Round(((float)CurrentCharge / MaxCharge) * 100, 1); }
        }

        #endregion

        #region Constructor

        public Battery()
        {
            
        }

        #endregion
    }
}
