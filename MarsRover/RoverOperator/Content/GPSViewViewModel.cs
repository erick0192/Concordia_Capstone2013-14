using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;
using MarsRover;

namespace RoverOperator.Content
{
    class GPSViewViewModel : INotifyPropertyChanged
    {
        #region Properties

        public String Title
        {
            get
            {
                return "Location"; //Temporary
            }

        }

        private GPSMap gpsMap;

        private GPSCoordinates roverCoordinates;
        public GPSCoordinates RoverCoordinates
        {
            get
            {
                return roverCoordinates;
            }
        }

        private List<GPSLocator> targetCoordinates;

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public GPSViewViewModel()
        {
            StatusUpdater.Instance.GPSCoordinatesUpdated += new StatusUpdater.GPSCoordinatesUpdatedDelegate(UpdateGPS);
        }

        #endregion

        #region Event Handlers

        //Updated coordinates from status update
        private void UpdateGPS(GPSCoordinates gpsCoordinates)
        {
            roverCoordinates = gpsCoordinates;
            
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("RoverCoordinates"));
            }
        }      

        //User adds new target to map
        private void addTarget(double targetLatitude, double targetLongitude)
        {
            if (targetCoordinates == null)
            {
                targetCoordinates = new List<GPSLocator>();
            }

            targetCoordinates.Add(new GPSLocator(targetLatitude, targetLongitude));
        }

        #endregion

        
    }

    #region Class Definitions
    /// <summary>
    /// Class which represents the map itself
    /// </summary>
    class GPSMap
    {
        #region Properties
        private string mapImageURL;
        private double minLongitude, maxLongitude, minLatitude, maxLatitude;
        #endregion

        #region Constructors
        public GPSMap(string mapImageURL)
        {
            this.mapImageURL = mapImageURL;
        }
        #endregion

    }

    /// <summary>
    /// Class used to define targets on the map (non-rover coordinates)
    /// </summary>
    class GPSLocator
    {
        #region Properties
        private double longitude, latitude;
        public double Longitude
        {
            get
            {
                return longitude;
            }
        }

        public double Latitude
        {
            get
            {
                return latitude;
            }
        }

        #endregion

        #region Constructors
        public GPSLocator(double latitude, double longitude)
        {
            this.longitude = latitude;
            this.latitude = longitude;
        }

        public GPSLocator(GPSCoordinates gpsCoordinates)
        {
            //parse longitude and latitude from gpsCoordinates
            this.longitude = nmeaToLongitude(gpsCoordinates.X, gpsCoordinates.Y);
            this.latitude = nmeaToLatitude(gpsCoordinates.Z);
        }
        #endregion

        #region Methods
        //NMEA in X, Y, Z format methods for longitude and latitude
        private double nmeaToLatitude(double Z)
        {
            return Math.Asin(Z / 6371); //6371 is approximate radius of the earth in km
        }
        private double nmeaToLongitude(double X, double Y)
        {
            return Math.Atan2(Y, X);
        }

        //Raw NMEA string parse method to extract longitude or latitude in decimal form
        private double parseNmea(string nmeaCoordinates)
        {
            double decimalCoordinates = 0;

            string degreeString;
            string minuteString;

            try
            {

                if (nmeaCoordinates.Contains('.'))
                {
                    if (nmeaCoordinates.Split('.').Length > 2)
                    {
                        //error
                    }

                    try
                    {
                        //get degrees
                        degreeString = nmeaCoordinates.Substring(0, nmeaCoordinates.IndexOf('.') - 3); //split at decimal point, select degree representing characters
                    }
                    catch
                    {
                        degreeString = "0"; // 0 degrees is a rare case
                    }

                    try
                    {
                        minuteString = nmeaCoordinates.Substring(nmeaCoordinates.IndexOf('.') - 2); // select last 2 characters before the decimal point until the end of the string, which represent minutes (if there are 2 digits of minutes)
                    }
                    catch
                    {
                        minuteString = nmeaCoordinates.Substring(nmeaCoordinates.IndexOf('.') - 1); // if there are 1 digit of minutes (rare case)
                    }

                    decimalCoordinates = Convert.ToDouble(degreeString) + (Convert.ToDouble(minuteString) / 60); // divide minutes by 60 to give degrees
                }
                else
                {
                    if (nmeaCoordinates.Length > 2)
                    {
                        //get degrees
                        degreeString = nmeaCoordinates.Remove(nmeaCoordinates.Length - 2); //remove last 2 characters which represent whole minutes

                        // get minutes
                        minuteString = nmeaCoordinates.Substring(nmeaCoordinates.Length - 2); // select last 2 characters which represent minutes

                        decimalCoordinates = Convert.ToDouble(degreeString) + (Convert.ToDouble(minuteString) / 60); // divide minutes by 60 to give degrees
                    }
                    else
                    {
                        //last 2 characters are always minutes, therefore we don't have degrees
                        minuteString = nmeaCoordinates;
                        degreeString = "0";

                        decimalCoordinates = Convert.ToDouble(minuteString) / 60; // divide minutes by 60 to give degrees
                    }
                }
            }
            catch
            {
                //something went wrong
                decimalCoordinates = 0;
            }

            return decimalCoordinates;
        }

        #endregion
    }
     
    #endregion
}
