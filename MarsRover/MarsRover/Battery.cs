using MarsRover.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarsRover
{
    public class Battery : IUpdateable
    {
        //Amperes
        public const float MIN_CURRENT = 0.0F;
        public const float MAX_CURRENT = 300.0f;

        //Celsius
        public const float MIN_TEMPERATURE = 0.0f;
        public const float MAX_TEMPERATYRE = 120.0f;

        private string regex;

        #region Properties

        public int CurrentCharge { get; set; }
        public int MaxCharge { get; set; }
        public float Temperature { get; set; }
        public float ChargePerc { get; set; }
        public float Current { get; set; }

        public float ChargeRatio
        {
            get { return (float)Math.Round(((float)CurrentCharge / MaxCharge), 2); }
        }

        public float ChargePercentage
        {
            get { return (float)Math.Round(((float)CurrentCharge / MaxCharge) * 100, 2); }
        }

        private List<BatteryCell> cells = new List<BatteryCell>(7);
        public List<BatteryCell> Cells { get { return cells; } }

        #endregion
       

        #region Constructor

        public Battery(int maxCharge)
        {
            MaxCharge = maxCharge;
            CurrentCharge = maxCharge;
            regex = @"<B;\d+(\.\d{1,3})?,\d+(\.\d{1,3})?,\d+(\.\d{1,3})?>";

            for(int i = 0; i < cells.Capacity; i++)
            {
                cells.Add(new BatteryCell(i + 1) { Voltage = 3.5f});                
            }
        }

        #endregion

        #region Methods

        private bool IsValidUpdateString(string input)
        {
            return Regex.IsMatch(input, regex);
        }

        public void UpdateFromString(string updateString)
        {
            if (IsValidUpdateString(updateString))
            {
                int posIdentifer = updateString.IndexOf(";");
                int length = updateString.Length - posIdentifer - 2;

                var updateArray = updateString.Substring(posIdentifer + 1, length).Split(',');

                this.ChargePerc = float.Parse(updateArray[0]);
                this.Current = float.Parse(updateArray[1]);
                this.Temperature = float.Parse(updateArray[2]);
            }
            else
            {
                throw new InvalidUpdateStringException(updateString);
            }
        }

        public string GetUpdateString()
        {
            return String.Format("<B;{0},{1},{2}>", Math.Round(ChargePerc, 3), Math.Round(Current, 3), Math.Round(Temperature, 3));
        }

        #endregion
    }
}
