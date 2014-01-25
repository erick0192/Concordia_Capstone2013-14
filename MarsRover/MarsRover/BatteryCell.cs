using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover
{
    public class BatteryCell : INotifyPropertyChanged
    {
        public enum CellStatus
        {
            UnderVoltage,
            Normal,
            Warning,
            OverVoltage
        }

        public static float MIN_VOLTAGE = 3.0f;
        public static float MIN_WARNING_VOLTAGE = 3.2f;
        public static float MAX_VOLTAGE = 4.2f;
        public static float MAX_WARNING_VOLTAGE = 4.0f;

        #region Properties

        public int CellID { get; set; }
        public BatteryCell.CellStatus Status { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public BatteryCell(int id)
        {
            CellID = id;    
        }

        private void UpdateCellStatus()
        {
            if(voltage > MAX_VOLTAGE)
            {
                if (Status != CellStatus.OverVoltage)
                {
                    Status = CellStatus.OverVoltage;   
                    if(OverVoltageDetected != null)
                    {
                        OverVoltageDetected(this);
                    }
                }
            }
            else if(voltage < MIN_VOLTAGE)
            {
                if(Status != CellStatus.UnderVoltage)
                {
                    Status = CellStatus.UnderVoltage;
                    if(UnderVoltageDetected != null)
                    {
                        UnderVoltageDetected(this);
                    }
                }                
            }
            else if(voltage <= MIN_WARNING_VOLTAGE || voltage >= MAX_WARNING_VOLTAGE)
            {
                if (Status != CellStatus.Warning)
                {                    
                    Status = CellStatus.Warning;
                    if (WarningVoltageDetected != null)
                    {
                        WarningVoltageDetected(this);
                    }
                }
            }
            else
            {
                if (Status != CellStatus.Normal)
                {                    
                    Status = CellStatus.Normal;
                    if (NormalVoltageDetected != null)
                    {
                        NormalVoltageDetected(this);
                    }
                }
            }
        }
    }
}
