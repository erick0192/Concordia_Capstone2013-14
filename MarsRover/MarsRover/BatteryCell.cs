using MarsRover.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarsRover
{
    public class BatteryCell : AbstractUpdateableComponent
    {
        public enum VoltageStatus
        {
            UnderVoltage,
            Normal,
            Warning,
            OverVoltage
        }

        //Volts
        public const float MIN_VOLTAGE = 3.0f;
        public const float MIN_WARNING_VOLTAGE = 3.2f;
        public const float MAX_VOLTAGE = 4.2f;
        public const float MAX_WARNING_VOLTAGE = 4.0f;

        #region Properties

        public int CellID { get; set; }
        public BatteryCell.VoltageStatus StatusVoltage { get; set; }

        private float voltage;
        public float Voltage
        {
            get
            {
                return voltage;
            }
            set
            {
                voltage = value;
                UpdateCellStatus();
            }
        }

        public override string UpdateIdentifier
        {
            get { return MarsRover.Commands.CommandMetadata.Update.BatteryCellIdentifier; }
        }

        #endregion

        #region Delegates and Events      

        public delegate void UnderVoltageDetectedDelegate(BatteryCell batteryCell);
        public event UnderVoltageDetectedDelegate UnderVoltageDetected;

        public delegate void OverVoltageDetectedDelegate(BatteryCell batteryCell);
        public event OverVoltageDetectedDelegate OverVoltageDetected;

        public delegate void NormalVoltageDetectedDelegate(BatteryCell batteryCell);
        public event NormalVoltageDetectedDelegate NormalVoltageDetected;

        public delegate void WarningVoltageDetectedDelegate(BatteryCell batteryCell);
        public event WarningVoltageDetectedDelegate WarningVoltageDetected;

        #endregion

        public BatteryCell(int id)
        {
            CellID = id;
            regex = "<" + UpdateIdentifier + @";[1-7],\d+(\.\d{1,3})?>";
        }

        private void UpdateCellStatus()
        {
            if (voltage > MAX_VOLTAGE && StatusVoltage != VoltageStatus.OverVoltage)
            {
                StatusVoltage = VoltageStatus.OverVoltage;
                if (OverVoltageDetected != null)
                {
                    OverVoltageDetected(this);
                }
            }
            else if (voltage < MIN_VOLTAGE && StatusVoltage != VoltageStatus.UnderVoltage)
            {
                StatusVoltage = VoltageStatus.UnderVoltage;
                if (UnderVoltageDetected != null)
                {
                    UnderVoltageDetected(this);
                }
            }
            else if ((voltage <= MIN_WARNING_VOLTAGE || voltage >= MAX_WARNING_VOLTAGE)
                && StatusVoltage != VoltageStatus.Warning)
            {
                StatusVoltage = VoltageStatus.Warning;
                if (WarningVoltageDetected != null)
                {
                    WarningVoltageDetected(this);
                }

            }
            else if (StatusVoltage != VoltageStatus.Normal)
            {
                StatusVoltage = VoltageStatus.Normal;
                if (NormalVoltageDetected != null)
                {
                    NormalVoltageDetected(this);
                }
            }
        }

        public static int GetCellIDFromUpdateString(string updateString)
        {
            var updateArray = GetUpdateStringArrayWithoutIdentifier(updateString);
            int id;
            if(!int.TryParse(updateArray[0], out id))
            {
                throw new InvalidUpdateStringException(updateString);
            }

            return id;
        }

        public override void UpdateFromString(string updateString)
        {
            if (IsValidUpdateString(updateString))
            {
                // We dont want to include the identifier nor the last bracket              
                var updateArray = GetUpdateStringArrayWithoutIdentifier(updateString);
                this.Voltage = float.Parse(updateArray[1]);
            }
            else
            {
                throw new InvalidUpdateStringException(updateString, "Make sure update string has the correct format and that the Cell ID is within range (1-7)");
            }
        }

        public override string GetUpdateString()
        {
            return CreateUpdateString(CellID, Voltage);
        }
    }
}
