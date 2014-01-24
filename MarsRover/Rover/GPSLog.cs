using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover
{
    public class GPSLog : ISensorLog
    {
        private string rawCommand;
        private const int expectedNumberOfArguments = 3;
        private string latitude;
        private string longitude;
        private string altitude;
        private bool isUpdated;
        private string rawData;
        private string identifier = "G";

        public string RawCommand { get { return rawCommand; } }
        public string Latitude { get { return latitude; } }
        public string Longitude { get { return longitude; } }
        public string Altitude { get { return altitude; } }
        public string Identifier { get { return Identifier; } }
        public bool IsUpdated { get { return isUpdated; } }

        public GPSLog(string unparsedText)
        {
            if (IsValidHeader(unparsedText))
            {
                rawCommand = unparsedText;
                rawData = TrimHeader(unparsedText);

                string[] values = rawData.Split(',');
                if (values.Length < expectedNumberOfArguments)
                {
                    throw new ArgumentException("GPS Command: Less arguments received than expected (" + values.Length + ") received");
                }
                latitude = values[0];
                longitude = values[1];
                altitude = values[2];
                isUpdated = true;
            }
            else
            { 
                throw new ArgumentException("Invalid command header received for GPS Log (" + unparsedText + ") received.");
            }
        }

        public void LogData()
        {
            //implementation
            isUpdated = false;
        }

        public void UpdateValues()
        {
            //implementation
            isUpdated = true;
            ReconstructCommand();
        }


        private string TrimHeader(string unparsedText)
        {
            return unparsedText.Substring(1);
        }

        private bool IsValidHeader(string unparsedText)
        {
            if(unparsedText.Substring(0,1) == "G") //Refactor: Move up
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReconstructCommand()
        {
            rawCommand = latitude + "," + longitude + "," + altitude + ",";
            if (isUpdated)
            {
                rawCommand += "1";
            }
            else
            {
                rawCommand += "0";
            }
        }
    }
}
